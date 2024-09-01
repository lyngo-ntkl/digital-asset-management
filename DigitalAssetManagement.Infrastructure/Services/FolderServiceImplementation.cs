using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
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

        public async Task Delete(int id)
        {
            User user = await _userService.GetLoginUserAsync();
            if (!await HasModifiedPermission(user.Id!.Value, id, null))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedFolderModification);
            }

            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(id);
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            _unitOfWork.FolderRepository.Delete(folder);
            await _unitOfWork.SaveAsync();
        }

        public async Task<FolderDetailResponseDto> Get(int id)
        {
            User user = await _userService.GetLoginUserAsync();

            if (!await _permissionService.HasReaderPermission(userId: user.Id!.Value, fileIdOrFolderIdOrDriveId: id))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedFolderAccess);
            }

            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(id);
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        public async Task<FolderDetailResponseDto> MoveFolder(int id, FolderMovementRequestDto request)
        {
            User user = await _userService.GetLoginUserAsync();

            if (!await _permissionService.HasAdminPermission(userId: user.Id!.Value, fileIdOrDriveIdOrFolderId: request.ParentFolderId ?? request.ParentDriveId!.Value, isDrive: request.ParentDriveId != null))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedFolderMovement);
            }

            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(id);
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            folder = _mapper.Map(request, folder);
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        private async Task<bool> HasModifiedPermission(int userId, int? folderId, int? driveId)
        {
            if (folderId != null)
            {
                var userPermissionToAccessFolder = await _unitOfWork.PermissionRepository.GetFirstOnConditionAsync(permission => permission.FolderId == folderId && permission.UserId == userId);
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
                if (drive.OwnerId == userId)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
