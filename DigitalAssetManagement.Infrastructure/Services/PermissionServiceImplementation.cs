using AutoMapper;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using System.Reflection;

namespace DigitalAssetManagement.Infrastructure.Services
{
    public class PermissionServiceImplementation : PermissionService
    {
        private readonly UnitOfWork _unitOfWork;

        public PermissionServiceImplementation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
