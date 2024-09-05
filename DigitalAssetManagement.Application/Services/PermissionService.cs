using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Application.Services
{
    public interface PermissionService
    {
        Task CreateFolderPermission(int fileIdOrFolderId, PermissionRequestDto request);
        Task CreatePermission(int userId, int assetId, Role role, Type assetType, bool hasChild = false, LTree? parentLTree = null);
        Task DuplicatePermissions(int childId, int parentId, Type parentType, Type childType);
        Task<bool> HasPermission(Role role, int userId, int assetId, Type assetType);
        Task<bool> HasPermissionLoginUser(Role role, int assetId, Type assetType);
    }
}
