using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.UseCases.Users.Create
{
    public class UserRegistrationHandler(
        UserRepository userRepository, 
        MetadataRepository metadataRepository, 
        PermissionRepository permissionRepository,
        HashingHelper hashingHelper,
        SystemFolderHelper folderHelper): UserRegistration
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly PermissionRepository _permissionRepository = permissionRepository;
        private readonly HashingHelper _hashingHelper = hashingHelper;
        private readonly SystemFolderHelper _systemFolderHelper = folderHelper;

        public async Task Register(RegistrationRequest request)
        {
            if (await _userRepository.ExistByEmailAsync(request.Email))
            {
                throw new BadRequestException(ExceptionMessage.RegisteredEmail);
            }

            var user = await AddUserAsync(request);

            await AddDriveAsync(user.Id);
        }

        public async Task<User> AddUserAsync(RegistrationRequest request)
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

        public async Task AddDriveAsync(int ownerId)
        {
            _systemFolderHelper.AddFolder(ownerId.ToString(), out string absolutePath);

            var metadata = new Metadata
            {
                Type = Entities.Enums.MetadataType.Drive,
                AbsolutePath = absolutePath,
                Name = $"{ownerId}",
                OwnerId = ownerId
            };
            await _metadataRepository.AddAsync(metadata);

            var permission = new Permission
            {
                UserId = ownerId,
                MetadataId = metadata.Id,
                Role = Entities.Enums.Role.Admin
            };
            await _permissionRepository.AddAsync(permission);
        }
    }
}
