using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.UnitOfWork
{
    public interface IMetadataPermissionUnitOfWork
    {
        IMetadataRepository MetadataRepository { get; }
        IPermissionRepository PermissionRepository { get; }
    }
}
