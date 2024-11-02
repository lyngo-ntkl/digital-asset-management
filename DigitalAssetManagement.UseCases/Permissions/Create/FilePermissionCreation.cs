namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public interface FilePermissionCreation
    {
        Task AddOrUpdateFilePermissionAsync(PermissionCreationRequest request);
    }
}
