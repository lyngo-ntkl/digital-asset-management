using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Application.Services
{
    public interface PermissionService
    {
        Task<Permission> Add(Permission permission);
        Task AddFolderPermission(string folderAbsolutePath, int userId, Role role);
        Task DuplicatePermissions(int childId, int parentId);
        Task<Permission?> GetPermissionByUserIdAndMetadataId(int userId, int metadataId);
        Task<bool> HasPermission(Role role, int userId, int metadataId);
        Task UpdatePermission(Permission permission);
    }
}
