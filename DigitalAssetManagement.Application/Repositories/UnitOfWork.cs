namespace DigitalAssetManagement.Application.Repositories
{
    public interface UnitOfWork
    {
        int Save();
        Task<int> SaveAsync();
        UserRepository UserRepository { get; }
        MetadataRepository DriveRepository { get; }
        PermissionRepository PermissionRepository { get; }
    }
}
