using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class PermissionServiceImplementation : PermissionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly UserService _userService;

        public PermissionServiceImplementation(UnitOfWork unitOfWork, UserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        private async Task CreateChildPermissions(int userId, Role role, LTree parentLtree)
        {
            var folders = await _unitOfWork.FolderRepository.GetAllAsync(filter: folder => folder.HierarchicalPath!.Value.IsDescendantOf(parentLtree) && folder.HierarchicalPath != parentLtree);
            var files = await _unitOfWork.FileRepository.GetAllAsync(filter: file => file.HierarchicalPath!.Value.IsDescendantOf(parentLtree));

            List<Permission> permissions = new List<Permission>(folders.Count + files.Count);
            foreach (var folder in folders)
            {
                permissions.Add(new Permission { UserId = userId, FolderId = folder.Id, Role = role });
            }
            foreach (var file in files)
            {
                permissions.Add(new Permission { UserId = userId, FileId = file.Id, Role = role });
            }

            await _unitOfWork.PermissionRepository.BatchInsertAsync(permissions);
            await _unitOfWork.SaveAsync();
        }

        private async Task CreateDrivePermission(int userId, int driveId, Role role)
        {
            var permission = new Permission { UserId = userId, DriveId = driveId, Role = role };
            await _unitOfWork.PermissionRepository.InsertAsync(permission);
            await _unitOfWork.SaveAsync();
        }

        private async Task CreateFilePermission(int userId, int fileId, Role role)
        {
            var permission = new Permission { UserId = userId, FileId = fileId, Role = role };
            await _unitOfWork.PermissionRepository.InsertAsync(permission);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateFolderPermission(int folderId, PermissionRequestDto request)
        {
            User loginUser = await _userService.GetLoginUserAsync();

            if (!await HasPermission(role: Role.Admin, userId: loginUser.Id!.Value, assetId: folderId, typeof(Folder)))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedModification);
            }

            var permissionUser = _unitOfWork.UserRepository.GetByEmail(request.Email);
            if (permissionUser == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }

            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(folderId);
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            await CreatePermission(permissionUser.Id!.Value, folderId, request.Role, typeof(Folder), true, folder.HierarchicalPath);
        }

        private async Task CreateFolderPermission(int userId, int folderId, Role role)
        {
            var permission = new Permission { UserId = userId, FolderId = folderId, Role = role };
            await _unitOfWork.PermissionRepository.InsertAsync(permission);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreatePermission(int userId, int assetId, Role role, Type assetType, bool hasChild = false, LTree? parentLTree = null)
        {
            if (assetType == typeof(Drive))
            {
                await CreateDrivePermission(userId, assetId, role);
            }
            else if (assetType == typeof(Folder))
            {
                await CreateFolderPermission(userId, assetId, role);
            }
            else if (assetType == typeof(Domain.Entities.File))
            {
                await CreateFilePermission(userId, assetId, role);
            }
            else
            {
                throw new Exception(ExceptionMessage.UnsupportedAssetType);
            }

            if (hasChild && parentLTree != null)
            {
                await CreateChildPermissions(userId, role, parentLTree.Value);
            }
        }

        public async Task DuplicatePermissions(int childId, int parentId, Type parentType, Type childType)
        {
            var childPropertyInfo = GetPermissionAssetProperty(childType);
            var parentPropertyInfo = GetPermissionAssetProperty(parentType);

            var permissions = await GetPermissions(assetId: parentId, assetType: parentType, false);
            foreach (var permission in permissions)
            {
                permission.Id = null;
                parentPropertyInfo!.SetValue(permission, null);
                childPropertyInfo!.SetValue(permission, childId);
            }

            await _unitOfWork.PermissionRepository.BatchInsertAsync(permissions);
            await _unitOfWork.SaveAsync();
        }

        private async Task<Permission?> GetPermission(int userId, int assetId, Type assetType)
        {
            var parameter = Expression.Parameter(typeof(Permission));
            Expression expression = Expression.Equal(
                Expression.Constant(userId),
                Expression.Property(parameter, nameof(Permission.UserId))
            );

            var id = Expression.Convert(Expression.Constant(assetId), typeof(int?));
            if (assetType == typeof(Domain.Entities.File))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FileId)),
                        id
                    )
                );
            }
            else if (assetType == typeof(Folder))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FolderId)),
                        id
                    )
                );
            }
            else if (assetType == typeof(Drive))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.DriveId)),
                        id
                    )
                );
            }
            else
            {
                throw new Exception(ExceptionMessage.UnsupportedAssetType);
            }

            var permission = await _unitOfWork.PermissionRepository.GetFirstOnConditionAsync(Expression.Lambda<Func<Permission, bool>>(expression, parameter));
            return permission;
        }

        private PropertyInfo? GetPermissionAssetProperty(Type assetType)
        {
            PropertyInfo? propertyInfo;
            if (assetType == typeof(Drive))
            {
                propertyInfo = typeof(Permission).GetProperty(nameof(Permission.DriveId));
            }
            else if (assetType == typeof(Folder))
            {
                propertyInfo = typeof(Permission).GetProperty(nameof(Permission.FolderId));
            }
            else if (assetType == typeof(Domain.Entities.File))
            {
                propertyInfo = typeof(Permission).GetProperty(nameof(Permission.FileId));
            }
            else
            {
                throw new Exception(ExceptionMessage.UnsupportedAssetType);
            }
            return propertyInfo;
        }

        private async Task<ICollection<Permission>> GetPermissions(int assetId, Type assetType, bool isTracked = true)
        {
            var parameter = Expression.Parameter(typeof(Permission));
            Expression expression = Expression.Constant(true);

            var id = Expression.Convert(Expression.Constant(assetId), typeof(int?));
            if (assetType == typeof(Domain.Entities.File))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FileId)),
                        id
                    )
                );
            }
            else if (assetType == typeof(Folder))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FolderId)),
                        id
                    )
                );
            }
            else if (assetType == typeof(Drive))
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.DriveId)),
                        id
                    )
                );
            }
            else
            {
                throw new Exception(ExceptionMessage.UnsupportedAssetType);
            }

            var permissions = await _unitOfWork.PermissionRepository.GetAllAsync(filter: Expression.Lambda<Func<Permission, bool>>(expression, parameter), isTracked: isTracked);
            return permissions;
        }

        public async Task<bool> HasPermission(Role role, int userId, int assetId, Type assetType)
        {
            var permission = await GetPermission(userId, assetId, assetType);
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

        public async Task<bool> HasPermissionLoginUser(Role role, int assetId, Type assetType)
        {
            User user = await _userService.GetLoginUserAsync();
            return await HasPermission(role, user.Id!.Value, assetId, assetType);
        }

    }
}