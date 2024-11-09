using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class MoveFolderHandler(IMetadataPermissionUnitOfWork unitOfWork, ISystemFolderHelper systemFolderHelper): MoveFolder
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISystemFolderHelper _systemFolderHelper = systemFolderHelper;

        public async Task MoveFolder(MoveFolderRequest request)
        {
            // move real folder & its file
            var folder = await GetFolderMetadataAsync(request.FolderId);
            var newParent = await GetParentMetadataAsync(request.NewParentId);
            var newAbsolutePath = AbsolutePathCreationHelper.ChangeParentPath(folder.AbsolutePath, newParent.AbsolutePath);
            _systemFolderHelper.MoveFolder(folder.AbsolutePath, newAbsolutePath);

            // modify folder metadata:
            //   for folder itself: parent id absolute path
            //   for children: absolute path
            folder.ParentId = request.NewParentId;
            folder.AbsolutePath = newAbsolutePath;
            await _unitOfWork.MetadataRepository.UpdateAsync(folder);

            var childrenMetadataIds = await _unitOfWork.MetadataRepository.GetMetadataIdByParentIdAsync(request.FolderId);
            await _unitOfWork.MetadataRepository.UpdateAbsolutePathByIdsAsync(childrenMetadataIds, newAbsolutePath);

            // permissions of folder & its children follow permission in new parent
            var folderAndChildrenMetadataIds = new List<int>(childrenMetadataIds.Count + 1);
            folderAndChildrenMetadataIds.AddRange(childrenMetadataIds);
            folderAndChildrenMetadataIds.Add(request.FolderId);
            await UpdatePermissionsAsync(folderAndChildrenMetadataIds, request.NewParentId);
        }

        private async Task<Metadata> GetFolderMetadataAsync(int folderId)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(folderId);
            if (metadata == null || metadata.Type != MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }

        public async Task<Metadata> GetParentMetadataAsync(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);

            if (metadata == null
                || metadata.Type != MetadataType.Folder && metadata.Type != MetadataType.Drive)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }

            return metadata;
        }

        private async Task UpdatePermissionsAsync(ICollection<int> folderAndChildrenMetadataIds, int newParentId)
        {
            await _unitOfWork.PermissionRepository.DeleteByMetadataIdsAsync(folderAndChildrenMetadataIds);
            await DuplicatePermissionsAsync(folderAndChildrenMetadataIds, newParentId);
        }

        private async Task DuplicatePermissionsAsync(ICollection<int> childrenIds, int parentId)
        {
            var parentPermissions = _unitOfWork.PermissionRepository.GetByMetadataIdNoTracking(parentId);

            var childrenPermissions = new List<Permission>(parentPermissions.Count * childrenIds.Count);
            foreach (int childId in childrenIds)
            {
                foreach (var permission in parentPermissions)
                {
                    permission.Id = 0;
                    permission.MetadataId = childId;
                    childrenPermissions.Add(permission);
                }
            }

            await _unitOfWork.PermissionRepository.AddRangeAsync(childrenPermissions);
        }
    }
}
