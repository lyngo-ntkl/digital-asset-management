using AutoMapper;
using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
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

        public async Task CreateFolderPermission(int folderId, PermissionRequestDto request)
        {
            User loginUser = await _userService.GetLoginUserAsync();

            if (!await HasPermission(role: Role.Admin, userId: loginUser.Id!.Value, fileIdOrDriveIdOrFolderId: folderId))
            {
                throw new ForbiddenException(ExceptionMessage.UnallowedMovement);
            }

            var user = _unitOfWork.UserRepository.GetByEmail(request.Email);
            if (user == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }

            var folder = await _unitOfWork.FolderRepository.GetByIdAsync(folderId);
            if (folder == null)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            List<Permission> permissions = new List<Permission>();
            foreach (var fileId in folder.Files.Select(file => file.Id))
            {
                Permission filePermission = new Permission { UserId = user.Id!.Value, Role = request.Role, FileId = fileId };
                permissions.Add(filePermission);
            }
            foreach (var subFolderId in folder.SubFolders.Select(subFolder => subFolder.Id))
            {
                Permission filePermission = new Permission { UserId = user.Id!.Value, Role = request.Role, FolderId = subFolderId };
                permissions.Add(filePermission);
            }
            Permission permission = new Permission { UserId = user.Id!.Value, Role = request.Role, FolderId = folderId };
            permissions.Add(permission);

            await _unitOfWork.PermissionRepository.BatchInsertAsync(permissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreatePermission(int userId, int assetId, int? folderId, int? driveId, bool isFile = true)
        {
            PropertyInfo? assetProperty;
            if(isFile)
            {
                assetProperty = typeof(Permission).GetProperty(nameof(Permission.FileId));
            } else
            {
                assetProperty = typeof(Permission).GetProperty(nameof(Permission.FolderId));
            }

            if (driveId != null)
            {
                var assetPermission = new Permission
                {
                    UserId = userId,
                    Role = Role.Admin,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                assetProperty?.SetValue(assetPermission, assetId);
                await _unitOfWork.PermissionRepository.InsertAsync(assetPermission);
            }

            if (folderId != null)
            {
                var folderPermissions = await _unitOfWork.PermissionRepository.GetAllAsync(permission => permission.FolderId == folderId, isTracked: false);
                foreach (var permission in folderPermissions)
                {
                    permission.Id = null;
                    assetProperty?.SetValue(permission, assetId);
                }
                await _unitOfWork.PermissionRepository.BatchInsertAsync(folderPermissions);
            }

            await _unitOfWork.SaveAsync();
        }

        private async Task<Permission?> GetPermission(int userId, int fileIdOrFolderId, bool isFile)
        {
            var parameter = Expression.Parameter(typeof(Permission));
            Expression expression = Expression.Equal(
                Expression.Constant(userId),
                Expression.Property(parameter, nameof(Permission.UserId))
            );

            var id = Expression.Convert(Expression.Constant(fileIdOrFolderId), typeof(int?));
            if (isFile)
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FileId)),
                        id
                    )
                );
            }
            else
            {
                expression = Expression.And(
                    expression,
                    Expression.Equal(
                        Expression.Property(parameter, nameof(Permission.FolderId)),
                        id
                    )
                );
            }

            var permission = await _unitOfWork.PermissionRepository.GetFirstOnConditionAsync(Expression.Lambda<Func<Permission, bool>>(expression, parameter));
            return permission;
        }

        private async Task<Role?> GetPermissionRole(int userId, int fileIdOrFolderIdOrDriveId, bool isFile = false, bool isDrive = false)
        {
            if (isFile && isDrive)
            {
                throw new Exception();
            }

            if (isDrive && await IsDriveOwner(userId, fileIdOrFolderIdOrDriveId))
            {
                return Role.Admin;
            }
            
            if (!isDrive)
            {
                var permission = await GetPermission(userId, fileIdOrFolderIdOrDriveId, isFile);
                if (permission != null)
                {
                    return permission.Role;
                }
            }

            return null;
        }

        public async Task<bool> HasPermission(Role role, int userId, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false)
        {
            var userRole = await GetPermissionRole(userId, fileIdOrDriveIdOrFolderId, isFile, isDrive);
            switch (role)
            {
                case Role.Reader:
                    return userRole != null;
                case Role.Contributor:
                    return userRole != null && userRole != Role.Reader;
                case Role.Admin:
                    return userRole == Role.Admin;
                default:
                    return false;
            }
        }

        public async Task<bool> HasPermissionLoginUser(Role role, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false)
        {
            User user = await _userService.GetLoginUserAsync();
            return await HasPermission(role, user.Id!.Value, fileIdOrDriveIdOrFolderId, isFile, isDrive);
        }

        private async Task<bool> IsDriveOwner(int userId, int driveId)
        {
            var drive = await _unitOfWork.DriveRepository.GetByIdAsync(driveId);
            return drive != null && drive.OwnerId == userId;
        }

    }
}