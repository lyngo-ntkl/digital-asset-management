using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests.Folders;
using DigitalAssetManagement.Application.Dtos.Responses.Folders;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using Hangfire;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private const int DeleteWaitDays = 30;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public FolderServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, UserService userService, PermissionService permissionService, IBackgroundJobClient backgroundJobClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _permissionService = permissionService;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request)
        {
            User loginUser = await _userService.GetLoginUserAsync();
            if (!await _permissionService.HasPermission(Role.Contributor, userId: loginUser.Id!.Value, fileIdOrDriveIdOrFolderId: request.ParentFolderId ?? request.ParentDriveId!.Value, isDrive: request.ParentDriveId != null))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var folder = _mapper.Map<Folder>(request);
            folder = await _unitOfWork.FolderRepository.InsertAsync(folder);
            await _unitOfWork.SaveAsync();

            await _permissionService.CreatePermission(loginUser.Id!.Value, folder.Id!.Value, request.ParentFolderId, request.ParentDriveId, false);

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        public async Task DeleteFolder(int id)
        {
            var folder = await GetFolderAsync(id);
            _unitOfWork.FolderRepository.Delete(folder);
            await _unitOfWork.SaveAsync();
        }

        public async Task Delete(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, fileIdOrDriveIdOrFolderId: id))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }
            await DeleteFolder(id);
        }

        public async Task<FolderDetailResponseDto> Get(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(role: Role.Reader, fileIdOrDriveIdOrFolderId: id))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedAccess);
            }

            var folder = await GetFolderAsync(id);

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        private async Task<Folder> GetFolderAsync(int id)
        {
            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(id, includedProperties: $"{nameof(Folder.SubFolders)},{nameof(Folder.Files)},{nameof(Folder.Permissions)}");
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return folder;
        }

        public async Task<FolderDetailResponseDto> MoveFolder(int id, FolderMovementRequestDto request)
        {
            if (!await _permissionService.HasPermissionLoginUser(role: Role.Admin, fileIdOrDriveIdOrFolderId: request.ParentFolderId ?? request.ParentDriveId!.Value, isDrive: request.ParentDriveId != null))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedMovement);
            }

            var folder = await GetFolderAsync(id);

            folder = _mapper.Map(request, folder);
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        public async Task MoveToTrash(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, fileIdOrDriveIdOrFolderId: id))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var folder = await GetFolderAsync(id);

            folder.IsDeleted = true;
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            _backgroundJobClient.Schedule(() => this.DeleteFolder(id), TimeSpan.FromDays(DeleteWaitDays));
        }

        public async Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, fileIdOrDriveIdOrFolderId: id))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var folder = await GetFolderAsync(id);

            folder = _mapper.Map(request, folder);
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

    }
}
