namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public interface FolderPermissionCreation
    {
        Task AddFolderPermission(PermissionCreationRequest request);
    }
}
