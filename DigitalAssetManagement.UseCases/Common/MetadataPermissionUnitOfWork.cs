using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Common
{
    public interface MetadataPermissionUnitOfWork
    {
        MetadataRepository MetadataRepository { get; }
        PermissionRepository PermissionRepository { get; }
    }
}
