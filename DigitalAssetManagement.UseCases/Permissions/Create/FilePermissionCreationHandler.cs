using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public class FilePermissionCreationHandler(MetadataPermissionUnitOfWork unitOfWork, UserRepository userRepository): FilePermissionCreation
    {
        private readonly MetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserRepository _userRepository = userRepository;
        public async Task AddFilePermissionAsync(PermissionCreationRequest request)
        {
            if (!await _unitOfWork.MetadataRepository.ExistByIdAndTypeAsync(request.MetadataId, Entities.Enums.MetadataType.File))
            {
                throw new NotFoundException(ExceptionMessage.FileNotFound);
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);

            var permission = await _unitOfWork.PermissionRepository.GetByUserIdAndMetadataIdAsync(user.Id, request.MetadataId);
            if (permission != null)
            {
                permission.Role = request.Role;
                await _unitOfWork.PermissionRepository.UpdateAsync(permission);
            }
            else
            {
                permission = new Entities.DomainEntities.Permission
                {
                    MetadataId = request.MetadataId,
                    UserId = user.Id,
                    Role = request.Role,
                };
                await _unitOfWork.PermissionRepository.AddAsync(permission);
            }
        }
    }
}
