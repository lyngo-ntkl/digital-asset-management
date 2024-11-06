using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.UnitOfWork
{
    public class MetadataPermissionUnitOfWorkImplementation(
        IMetadataRepository metadataRepository,
        IPermissionRepository permissionRepository) : IMetadataPermissionUnitOfWork
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public IMetadataRepository MetadataRepository => _metadataRepository;

        public IPermissionRepository PermissionRepository => _permissionRepository;
    }
}
