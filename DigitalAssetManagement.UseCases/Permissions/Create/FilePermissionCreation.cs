namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public interface FilePermissionCreation
    {
        Task AddFilePermissionAsync(PermissionCreationRequest request);
    }
}
