using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.UnitOfWork;

namespace DigitalAssetManagement.UseCases.Files.Update
{
    public class MoveFileHandler(IMetadataPermissionUnitOfWork unitOfWork, ISystemFileHelper systemFileHelper): MoveFile
    {
        private readonly IMetadataPermissionUnitOfWork _unitOfWork = unitOfWork;
        private readonly ISystemFileHelper _systemFileHelper = systemFileHelper;

        public async Task MoveFile(MoveFileRequest request)
        {
            var newParent = await GetParentMetadataAsync(request.NewParentId);
            var file = await GetFileMetadataAsync(request.FileId);

            var newFileAbsolutePath = _systemFileHelper.MoveFile(file.AbsolutePath, newParent.AbsolutePath);

            file.ParentMetadataId = request.NewParentId;
            file.AbsolutePath = newFileAbsolutePath;
            await _unitOfWork.MetadataRepository.UpdateAsync(file);

            await UpdatePermissionsAsync(request.FileId, request.NewParentId);
        }

        private async Task<Metadata> GetFileMetadataAsync(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.Type != MetadataType.File)
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return metadata;
        }

        private async Task<Metadata> GetParentMetadataAsync(int id)
        {
            var metadata = await _unitOfWork.MetadataRepository.GetByIdAsync(id);
            if (metadata == null || 
                (metadata.Type != MetadataType.Folder && metadata.Type != MetadataType.Drive))
            {
                throw new NotFoundException(ExceptionMessage.FolderNotFound);
            }
            return metadata;
        }

        private async Task UpdatePermissionsAsync(int fileId, int newParentId)
        {
            await DeleteOldPermissions(fileId);
            await DuplicatePermissionsAsync(fileId, newParentId);
        }

        private async Task DeleteOldPermissions(int fileId)
        {
            await _unitOfWork.PermissionRepository.DeleteByMetadataId(fileId);
        }

        private async Task DuplicatePermissionsAsync(int fileId, int parentId)
        {
            var parentPermissions = _unitOfWork.PermissionRepository.GetByMetadataIdNoTracking(parentId);
            var childPermissions = new List<Permission>(parentPermissions.Count);
            foreach (var permissions in parentPermissions)
            {
                permissions.Id = 0;
                permissions.MetadataId = fileId;
                childPermissions.Add(permissions);
            }
            await _unitOfWork.PermissionRepository.AddRangeAsync(childPermissions);
        }
    }
}
