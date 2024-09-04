using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Services
{
    public interface PermissionService
    {
        Task CreateFolderPermission(int fileIdOrFolderId, PermissionRequestDto request);
        Task CreatePermission(int userId, int assetId, int? folderId, int? driveId, bool isFile = true);
        Task<bool> HasPermission(Role role, int userId, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false);
        Task<bool> HasPermissionLoginUser(Role role, int fileIdOrDriveIdOrFolderId, bool isFile = false, bool isDrive = false);
    }
}
