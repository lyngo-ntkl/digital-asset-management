using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class PermissionServiceImplementation(UnitOfWork unitOfWork) : PermissionService
    {
        private readonly UnitOfWork _unitOfWork = unitOfWork;

        public async Task<Permission> Add(Permission permission)
        {
            permission = await _unitOfWork.PermissionRepository.AddAsync(permission);
            await _unitOfWork.SaveAsync();
            return permission;
        }

        public async Task AddPermissionsWithDifferentUsers(int fileMetadataId, int newParentMetadataId)
        {
            var newParentPermissions = await _unitOfWork.PermissionRepository.GetAllAsync(p => p.MetadataId == newParentMetadataId, isTracked: false);
            var newParentPermissionUserIds = newParentPermissions.Select(p => p.UserId);
            var filePermissionUserIds = _unitOfWork.PermissionRepository.GetUserIdByMetadataId(fileMetadataId);
            var differenceUserIds = newParentPermissionUserIds.Except(filePermissionUserIds);

            var permissions = newParentPermissions.Where(p => differenceUserIds.Contains(p.UserId));
            foreach (var permission in permissions)
            {
                permission.Id = 0;
                permission.MetadataId = fileMetadataId;
            }

            await _unitOfWork.PermissionRepository.AddRangeAsync(permissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task DuplicatePermissionsAsync(int childMetadataId, int parentMetadataId)
        {
            var parentPermissions = await GetPermissions(parentMetadataId, false);
            foreach (var permission in parentPermissions)
            {
                permission.Id = 0;
                permission.MetadataId = childMetadataId;
            }

            await _unitOfWork.PermissionRepository.AddRangeAsync(parentPermissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task DuplicatePermissions(ICollection<int> childrenIds, int parentId)
        {
            var parentPermissions = await GetPermissions(parentId, false);

            List<Permission> childrenPermissions = new();
            foreach (int childId in childrenIds)
            {
                foreach (var permission in parentPermissions)
                {
                    permission.Id = 0;
                    permission.MetadataId = childId;
                    childrenPermissions.Add(permission);
                }
            }

            await _unitOfWork.PermissionRepository.AddRangeAsync(childrenPermissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Permission?> GetPermissionByUserIdAndMetadataId(int userId, int metadataId)
        {
            var permission = await _unitOfWork.PermissionRepository.GetByUserIdAndMetadataIdAsync(userId, metadataId);
            return permission;
        }

        private async Task<ICollection<Permission>> GetPermissions(int metadataId, bool isTracked = true)
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllAsync(filter: p => p.MetadataId == metadataId && !p.IsDeleted, isTracked: isTracked);
            return permissions;
        }

        public async Task<bool> HasPermission(Role role, int userId, int metadataId)
        {
            var permission = await GetPermissionByUserIdAndMetadataId(userId, metadataId);
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

        public async Task UpdatePermission(Permission permission)
        {
            _unitOfWork.PermissionRepository.Update(permission);
            await _unitOfWork.SaveAsync();
        }
    }
}