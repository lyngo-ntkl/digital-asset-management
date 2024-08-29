using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;

        public FolderServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, UserService userService, PermissionService permissionService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _permissionService = permissionService;
        }

        public async Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request)
        {
            User user = await _userService.GetLoginUserAsync();
            if (!await HasModifiedPermission(user.Id!.Value, request.ParentFolderId, request.ParentDriveId))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedFolderModification);
            }

            var folder = _mapper.Map<Folder>(request);
            folder = await _unitOfWork.FolderRepository.InsertAsync(folder);
            await _unitOfWork.SaveAsync();

            await _permissionService.CreatePermission(user.Id.Value, folder.Id!.Value, request.ParentFolderId, request.ParentDriveId, false);

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        private async Task<bool> HasModifiedPermission(int userId, int? folderId, int? driveId)
        {
            if (folderId != null)
            {
                var userPermissionToAccessFolder = await _unitOfWork.PermissionRepository.GetOnConditionAsync(permission => permission.FolderId == folderId && permission.UserId == userId);
                if (userPermissionToAccessFolder != null && userPermissionToAccessFolder.Role != Role.Reader)
                {
                    return true;
                }
            }

            if (driveId != null)
            {
                var drive = _unitOfWork.DriveRepository.GetById(driveId.Value);
                if (drive == null)
                {
                    throw new NotFoundException(ExceptionMessage.DriveNotFound);
                }
                if (drive.UserId == userId)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
