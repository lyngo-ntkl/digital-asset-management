using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class FolderNameModificationHandler(
        IMetadataRepository metadataRepository, 
        IMapper mapper,
        ISystemFolderHelper systemFolderHelper) : FolderNameModification
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ISystemFolderHelper _systemFolderHelper = systemFolderHelper;

        public async Task<FolderDetailResponse> RenameFolderAsync(MetadataNameModificationRequest request)
        {
            var folder = await GetFolderAsync(request.Id);
            var newAbsolutePath = AbsolutePathCreationHelper.ChangeName(request.NewName, folder.AbsolutePath);
            var oldAbsolutePath = folder.AbsolutePath;

            await UpdateFolderMetadataAsync(folder, request.NewName, newAbsolutePath);
            await UpdateChildrenMetadataAbsolutePathAsync(folder.Id, newAbsolutePath);
            RenamePhysicalFolder(oldAbsolutePath, newAbsolutePath);

            return _mapper.Map<FolderDetailResponse>(folder);
        }

        private async Task<Metadata> GetFolderAsync(int id)
        {
            var metadata = await _metadataRepository.GetByIdAsync(id);
            if (metadata == null || metadata.Type != Entities.Enums.MetadataType.Folder)
            {
                throw new NotFoundException(ExceptionMessage.MetadataNotFound);
            }
            return metadata;
        }

        private async Task UpdateFolderMetadataAsync(Metadata folder, string newName, string newAbsolutePath)
        {
            folder.Name = newName;
            folder.AbsolutePath = newAbsolutePath;
            await _metadataRepository.UpdateAsync(folder);
        }

        private async Task UpdateChildrenMetadataAbsolutePathAsync(int parentId, string parentAbsolutePath)
        {
            await _metadataRepository.UpdateAbsolutePathByParentIdAsync(parentId, parentAbsolutePath);
        }

        private void RenamePhysicalFolder(string oldAbsolutePath, string newAbsolutePath)
        {
            _systemFolderHelper.MoveFolder(oldAbsolutePath, newAbsolutePath);
        }
    }
}
