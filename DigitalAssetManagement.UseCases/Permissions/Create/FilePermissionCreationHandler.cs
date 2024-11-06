using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public class FilePermissionCreationHandler(IMetadataPermissionUnitOfWork unitOfWork, UserRepository userRepository): FilePermissionCreation
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserRepository _userRepository = userRepository;
        public async Task AddOrUpdateFilePermissionAsync(PermissionCreationRequest request)
        {
            await CheckFileExistanceAsync(request.MetadataId);
            var user = await GetUserAsync(request.Email);
            await AddOrUpdatePermissionAsync(user.Id, request.MetadataId, request.Role);
        }

        private async Task CheckFileExistanceAsync(int fileId)
        {
            if (!await _unitOfWork.MetadataRepository.ExistByIdAndTypeAsync(fileId, Entities.Enums.MetadataType.File))
            {
                throw new NotFoundException(ExceptionMessage.FileNotFound);
            }
        }

        private async Task<User> GetUserAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }
            return user;
        }

        private async Task AddOrUpdatePermissionAsync(int userId, int metadataId, Role role)
        {
            var permission = await _unitOfWork.PermissionRepository.GetByUserIdAndMetadataIdAsync(userId, metadataId);
            if (permission == null)
            {
                await AddPermissionAsync(userId, metadataId, role);
            }
            else
            {
                await UpdatePermissionAsync(permission, role);
            }
        }

        private async Task AddPermissionAsync(int userId, int metadataId, Role role)
        {
            var permission = new Permission
            {
                MetadataId = metadataId,
                UserId = userId,
                Role = role,
            };
            await _unitOfWork.PermissionRepository.AddAsync(permission);
        }

        private async Task UpdatePermissionAsync(Permission permission, Role role)
        {
            permission.Role = role;
            await _unitOfWork.PermissionRepository.UpdateAsync(permission);
        }
    }
}
