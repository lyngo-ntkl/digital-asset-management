using DigitalAssetManagement.Application.Common;
using DigitalAssetManagement.Application.Exceptions;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using System.IO;

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

        public async Task AddFolderPermission(string folderAbsolutePath, int userId, Role role)
        {
            var folderAndChildrenMetadata = await _unitOfWork.MetadataRepository.GetAllAsync(
                m => m.AbsolutePath.StartsWith(folderAbsolutePath)
            );

            // update existed permissions
            var folderAndChildrenMetadataIds = folderAndChildrenMetadata.Select(m => m.Id);
            var existedPermissions = await _unitOfWork.PermissionRepository.GetAllAsync(
                p => p.UserId == userId && folderAndChildrenMetadataIds.Contains(p.MetadataId)
            );
            var existedPermissionIds = existedPermissions.Select(m => m.Id);
            var updatedRow = await _unitOfWork.PermissionRepository.BatchUpdateAsync(
                p => p.SetProperty(entity => entity.Role, value => role), 
                filter: p => existedPermissionIds.Contains(p.Id)
            );
            await _unitOfWork.SaveAsync();
            var existedPermissionMetadataIds = existedPermissions.Select(p => p.MetadataId).ToList();

            var newPermissionMetadataIds = folderAndChildrenMetadataIds.Except(existedPermissionMetadataIds);
            
            List<Permission> newPermissions = new List<Permission>(folderAndChildrenMetadata.Count - updatedRow);
            foreach (var metadataId in newPermissionMetadataIds)
            {
                newPermissions.Add(
                    new Permission
                    {
                        MetadataId = metadataId,
                        UserId = userId,
                        Role = role
                    }
                );
            }

            await _unitOfWork.PermissionRepository.BatchAddAsync(newPermissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task DuplicatePermissions(int childMetadataId, int parentMetadataId)
        {
            var parentPermissions = await GetPermissions(parentMetadataId, false);
            foreach (var permission in parentPermissions)
            {
                permission.Id = 0;
                permission.MetadataId = childMetadataId;
            }

            await _unitOfWork.PermissionRepository.BatchAddAsync(parentPermissions);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Permission?> GetPermissionByUserIdAndMetadataId(int userId, int metadataId)
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