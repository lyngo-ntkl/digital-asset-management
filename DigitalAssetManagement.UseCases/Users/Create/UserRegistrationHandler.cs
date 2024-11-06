using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.UseCases.Users.Create
{
    public class UserRegistrationHandler(
        UserRepository userRepository, 
        IMetadataRepository metadataRepository, 
        IPermissionRepository permissionRepository,
        IHashingHelper hashingHelper,
        ISystemFolderHelper folderHelper): UserRegistration
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IHashingHelper _hashingHelper = hashingHelper;
        private readonly ISystemFolderHelper _systemFolderHelper = folderHelper;

        public async Task Register(RegistrationRequest request)
        {
            await CheckUserExistanceAsync(request.Email);
            var user = await AddUserAsync(request);
            await AddDriveAsync(user.Id);
        }

        private async Task CheckUserExistanceAsync(string email)
        {
            if (await _userRepository.ExistByEmailAsync(email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }
        }

        private async Task<User> AddUserAsync(RegistrationRequest request)
        {
            _hashingHelper.Hash(request.Password, out string salt, out string hash);
            var user = new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber
            };

            return await _userRepository.AddAsync(user);
        }

        private async Task AddDriveAsync(int ownerId)
        {
            var absolutePath = AddPhysicalDrive(ownerId);
            var driveId = await AddDriveMetadataAsync(absolutePath, ownerId);
            await AddDrivePermissionAsync(ownerId, driveId);
        }

        private string AddPhysicalDrive(int ownerId)
        {
            var absolutePath = AbsolutePathCreationHelper.CreateAbsolutePath(ownerId.ToString());
            _systemFolderHelper.AddFolder(absolutePath);
            return absolutePath;
        }

        private async Task<int> AddDriveMetadataAsync(string absolutePath, int ownerId)
        {
            var metadata = new Metadata
            {
                Type = Entities.Enums.MetadataType.Drive,
                AbsolutePath = absolutePath,
                Name = $"{ownerId}",
                OwnerId = ownerId
            };
            metadata = await _metadataRepository.AddAsync(metadata);
            return metadata.Id;
        }

        private async Task AddDrivePermissionAsync(int ownerId, int driveId)
        {
            var permission = new Permission
            {
                UserId = ownerId,
                MetadataId = driveId,
                Role = Entities.Enums.Role.Admin
            };
            await _permissionRepository.AddAsync(permission);
        }
    }
}
