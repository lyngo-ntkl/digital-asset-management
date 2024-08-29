namespace DigitalAssetManagement.Application.Services
{
    public interface PermissionService
    {
        Task CreatePermission(int userId, int assetId, int? folderId, int? driveId, bool isFile = true);
    }
}
