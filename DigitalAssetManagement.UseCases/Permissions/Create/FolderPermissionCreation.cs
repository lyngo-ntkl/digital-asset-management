namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public interface FolderPermissionCreation
    {
        Task AddFolderPermissionAsync(PermissionCreationRequest request);
    }
}
