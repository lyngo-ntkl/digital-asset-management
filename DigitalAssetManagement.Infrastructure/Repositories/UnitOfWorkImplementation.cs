using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class UnitOfWorkImplementation : UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserRepository _userRepository;
        private readonly DriveRepository _driveRepository;
        private readonly FolderRepository _folderRepository;
        private readonly FileRepository _fileRepository;

        public UnitOfWorkImplementation(ApplicationDbContext context, UserRepository userRepository, DriveRepository driveRepository, FolderRepository folderRepository, FileRepository fileRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _driveRepository = driveRepository;
            _folderRepository = folderRepository;
            _fileRepository = fileRepository;
        }

        public UserRepository UserRepository => _userRepository;

        public DriveRepository DriveRepository => _driveRepository;

        public FileRepository FileRepository => _fileRepository;

        public FolderRepository FolderRepository => _folderRepository;

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
