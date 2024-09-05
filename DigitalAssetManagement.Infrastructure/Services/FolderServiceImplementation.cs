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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class FolderServiceImplementation : FolderService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IConfiguration _configuration;

        public FolderServiceImplementation(UnitOfWork unitOfWork, IMapper mapper, UserService userService, PermissionService permissionService, IBackgroundJobClient backgroundJobClient, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
            _permissionService = permissionService;
            _backgroundJobClient = backgroundJobClient;
            _configuration = configuration;
        }

        public async Task<FolderDetailResponseDto> Create(FolderCreationRequestDto request)
        {
            User loginUser = await _userService.GetLoginUserAsync();
            int parentId = request.ParentFolderId ?? request.ParentDriveId!.Value;
            Type parentType = request.ParentFolderId != null ? typeof(Folder) : typeof(Drive);

            if (!await _permissionService.HasPermission(Role.Contributor, userId: loginUser.Id!.Value, assetId: parentId, assetType: parentType))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var folder = _mapper.Map<Folder>(request);
            folder = await _unitOfWork.FolderRepository.InsertAsync(folder);
            await _unitOfWork.SaveAsync();

            // TODO: why can't load navigation property here even though I has already set lazy loading proxies?
            LTree? parentHierarchicalPath;
            if (request.ParentFolderId != null)
            {
                var parentFolder = await _unitOfWork.FolderRepository.GetByIdAsync(request.ParentFolderId!.Value);
                parentHierarchicalPath = parentFolder?.HierarchicalPath;
            }
            else
            {
                var parentDrive = await _unitOfWork.DriveRepository.GetByIdAsync(request.ParentDriveId!.Value);
                parentHierarchicalPath = parentDrive?.HierarchicalPath;
            }
            folder.HierarchicalPath = new LTree($"{parentHierarchicalPath!.Value}.{folder.Id}");
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            await _permissionService.DuplicatePermissions(folder.Id!.Value, parentId, parentType, typeof(Folder));

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
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, assetId: id, typeof(Folder)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }
            await DeleteFolder(id);
        }

        public async Task<FolderDetailResponseDto> Get(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(role: Role.Reader, assetId: id, typeof(Folder)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedAccess);
            }

            var folder = await GetFolderAsync(id);

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        private async Task<Folder> GetFolderAsync(int id)
        {
            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(id, includedProperties: $"{nameof(Folder.ParentDrive)},{nameof(Folder.ParentFolder)},{nameof(Folder.SubFolders)},{nameof(Folder.Files)},{nameof(Folder.Permissions)}");
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return folder;
        }

        public async Task<FolderDetailResponseDto> MoveFolder(int id, FolderMovementRequestDto request)
        {
            if (!await _permissionService.HasPermissionLoginUser(role: Role.Admin, assetId: request.ParentFolderId ?? request.ParentDriveId!.Value, assetType: typeof(Folder)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedMovement);
            }

            var folder = await GetFolderAsync(id);

            // TODO: update permission
            folder = _mapper.Map(request, folder);
            _unitOfWork.FolderRepository.Update(folder);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FolderDetailResponseDto>(folder);
        }

        public async Task MoveToTrash(int id)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, assetId: id, assetType: typeof(Folder)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var folder = await GetFolderAsync(id);

            await _unitOfWork.FolderRepository.BatchUpdateAsync(folder1 => folder1.SetProperty(f => f.IsDeleted, f => true), filter: f => f.HierarchicalPath!.Value.IsDescendantOf(folder.HierarchicalPath!.Value));
            await _unitOfWork.FileRepository.BatchUpdateAsync(file => file.SetProperty(f => f.IsDeleted, f => true), filter: file => file.HierarchicalPath!.Value.IsDescendantOf(folder.HierarchicalPath!.Value));
            await _unitOfWork.SaveAsync();

            _backgroundJobClient.Schedule(() => this.DeleteFolder(id), TimeSpan.FromDays(int.Parse(_configuration["schedule:deletedWaitDays"]!)));
        }

        public async Task<FolderDetailResponseDto> Update(int id, FolderModificationRequestDto request)
        {
            if (!await _permissionService.HasPermissionLoginUser(Role.Contributor, assetId: id, typeof(Folder)))
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
