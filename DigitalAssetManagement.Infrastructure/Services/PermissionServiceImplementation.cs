using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class PermissionServiceImplementation : PermissionService
    {
        private readonly UnitOfWork _unitOfWork;

        public PermissionServiceImplementation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Permission> Add(Permission permission)
        {
            permission = await _unitOfWork.PermissionRepository.AddAsync(permission);
            await _unitOfWork.SaveAsync();
            return permission;
        }

        //private async Task CreateChildPermissions(int userId, Role role, LTree parentLtree)
        //{
        //    var folders = await _unitOfWork.FolderRepository.GetAllAsync(filter: folder => folder.HierarchicalPath!.Value.IsDescendantOf(parentLtree) && folder.HierarchicalPath != parentLtree);
        //    var files = await _unitOfWork.FileRepository.GetAllAsync(filter: file => file.HierarchicalPath!.Value.IsDescendantOf(parentLtree));
         
        //    List<Permission> permissions = new List<Permission>(folders.Count + files.Count);
        //    foreach (var folder in folders)
        //    {
        //        permissions.Add(new Permission { UserId = userId, FolderId = folder.Id, Role = role });
        //    }
        //    foreach (var file in files)
        //    {
        //        permissions.Add(new Permission { UserId = userId, FileId = file.Id, Role = role });
        //    }

        //    await _unitOfWork.PermissionRepository.BatchAddAsync(permissions);
        //    await _unitOfWork.SaveAsync();
        //}

        //private async Task CreateDrivePermission(int userId, int driveId, Role role)
        //{
        //    var permission = new Permission { UserId = userId, DriveId = driveId, Role = role };
        //    await _unitOfWork.PermissionRepository.AddAsync(permission);
        //    await _unitOfWork.SaveAsync();
        //}

        //private async Task CreateFilePermission(int userId, int fileId, Role role)
        //{
        //    var permission = new Permission { UserId = userId, FileId = fileId, Role = role };
        //    await _unitOfWork.PermissionRepository.AddAsync(permission);
        //    await _unitOfWork.SaveAsync();
        //}

        //public async Task CreateFolderPermission(int folderId, PermissionRequestDto request)
        //{
        //    User loginUser = await _userService.GetLoginUserAsync();

        //    if (!await HasPermission(role: Role.Admin, userId: loginUser.Id!.Value, assetId: folderId, typeof(Folder)))
        //    {
        //        throw new ForbiddenException(ExceptionMessage.UnallowedModification);
        //    }

        //    var permissionUser = _unitOfWork.UserRepository.GetByEmail(request.Email);
        //    if (permissionUser == null)
        //    {
        //        throw new NotFoundException(ExceptionMessage.UserNotFound);
        //    }

        //    var folder = await _unitOfWork.FolderRepository.GetByIdAsync(folderId);
        //    if (folder == null)
        //    {
        //        throw new NotFoundException(ExceptionMessage.FolderNotFound);
        //    }

        //    await CreatePermission(permissionUser.Id!.Value, folderId, request.Role, typeof(Folder), true, folder.HierarchicalPath);
        //}

        //private async Task CreateFolderPermission(int userId, int folderId, Role role)
        //{
        //    var permission = new Permission { UserId = userId, FolderId = folderId, Role = role };
        //    await _unitOfWork.PermissionRepository.AddAsync(permission);
        //    await _unitOfWork.SaveAsync();
        //}

        //public async Task CreatePermission(int userId, int assetId, Role role, Type assetType, bool hasChild = false, LTree? parentLTree = null)
        //{
        //    if (assetType == typeof(Drive))
        //    {
        //        await CreateDrivePermission(userId, assetId, role);
        //    }
        //    else if (assetType == typeof(Folder))
        //    {
        //        await CreateFolderPermission(userId, assetId, role);
        //    }
        //    else if (assetType == typeof(Domain.Entities.File))
        //    {
        //        await CreateFilePermission(userId, assetId, role);
        //    }
        //    else
        //    {
        //        throw new Exception(ExceptionMessage.UnsupportedAssetType);
        //    }

        //    if (hasChild && parentLTree != null)
        //    {
        //        await CreateChildPermissions(userId, role, parentLTree.Value);
        //    }
        //}

        public async Task DuplicatePermissions(int childMetadataId, int parentMetadataId)
        {
            var parentPermissions = await GetPermissions(parentMetadataId, false);
            foreach (var permission in parentPermissions)
            {
                permission.Id = null;
                permission.MetadataId = childMetadataId;
            }

            await _unitOfWork.PermissionRepository.BatchAddAsync(parentPermissions);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Permission?> GetPermission(int userId, int metadataId)
        {
            var permission = await _unitOfWork.PermissionRepository.GetFirstOnConditionAsync(p => p.MetadataId == metadataId && p.UserId == userId && !p.IsDeleted);
            return permission;
        }

        private async Task<ICollection<Permission>> GetPermissions(int metadataId, bool isTracked = true)
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllAsync(filter: p => p.MetadataId == metadataId && !p.IsDeleted, isTracked: isTracked);
            return permissions;
        }

        public async Task<bool> HasPermission(Role role, int userId, int metadataId)
        {
            var permission = await GetPermission(userId, metadataId);
            switch (role)
            {
                case Role.Reader:
                    return permission != null;
                case Role.Contributor:
                    return permission != null && permission.Role != Role.Reader;
                case Role.Admin:
                    return permission != null && permission.Role == Role.Admin;
                default:
                    return false;
            }
        }
    }
}