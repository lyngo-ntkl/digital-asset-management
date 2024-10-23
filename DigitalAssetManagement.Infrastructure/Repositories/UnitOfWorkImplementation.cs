using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class UnitOfWorkImplementation : UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly MetadataRepository _metadataRepository;
        private readonly PermissionRepository _permissionRepository;

        public UnitOfWorkImplementation(ApplicationDbContext context, UserRepository userRepository, MetadataRepository driveRepository, PermissionRepository permissionRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _metadataRepository = driveRepository;
            _permissionRepository = permissionRepository;
        }

        public UserRepository UserRepository => _userRepository;

        public MetadataRepository MetadataRepository => _metadataRepository;

        public PermissionRepository PermissionRepository => _permissionRepository;

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
