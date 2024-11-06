using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Permissions.Create
{
    public class FolderPermissionCreationHandler(IMetadataPermissionUnitOfWork unitOfWork, UserRepository userRepository) : FolderPermissionCreation
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserRepository _userRepository = userRepository;

        public async Task AddFolderPermissionAsync(PermissionCreationRequest request)
        {
            var user = await GetUserByEmailAsync(request.Email);
            await AddPermissionToFolderAndChildren(request.MetadataId, user.Id, request.Role);
        }

        private async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundException(ExceptionMessage.UserNotFound);
            }
            return user;
        }

        private async Task AddPermissionToFolderAndChildren(int folderId, int userId, Role role)
        {
            var folderAndChildrenIds = await _unitOfWork.MetadataRepository.GetMetadataIdByParentIdAsync(folderId);
            folderAndChildrenIds.Add(folderId);

            var existedPermission = await _unitOfWork.PermissionRepository.GetByUserIdAndMetadataIdsAsync(userId, folderAndChildrenIds);
            var existedPermissionIds = existedPermission.Select(x => x.Id);
            await UpdateExistedPermissions(existedPermissionIds, role);

            var existedPermissionMetadataIds = existedPermission.Select(x => x.MetadataId);
            await AddNewPermissions(folderAndChildrenIds.Except(existedPermissionIds), userId, role);
        }

        private async Task UpdateExistedPermissions(IEnumerable<int> ids, Role role)
        {
            await _unitOfWork.PermissionRepository.UpdateRoleByIdsAsync(role, ids);
        }

        private async Task AddNewPermissions(IEnumerable<int> metadataIds, int userId, Role role)
        {
            List<Permission> newPermissions = new(metadataIds.Count());
            foreach (int metadataId in metadataIds)
            {
                newPermissions.Add(
                    new Permission
                    {
                        MetadataId = metadataId,
                        UserId = userId,
                        Role = role
                    }
                );
            }

            await _unitOfWork.PermissionRepository.AddRangeAsync(newPermissions);
        }
    }
}
