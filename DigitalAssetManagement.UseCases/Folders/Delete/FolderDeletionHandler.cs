using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Common.Exceptions;

namespace DigitalAssetManagement.UseCases.Folders.Delete
{
    public class FolderDeletionHandler(MetadataRepository metadataRepository, SystemFolderHelper systemFolderHelper): FolderDeletion
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly SystemFolderHelper _systemFolderHelper = systemFolderHelper;

        public async Task DeleteFolder(int id)
        {
            Metadata metadata = await GetFolderAsync(id);
            _systemFolderHelper.DeleteFolder(metadata.AbsolutePath);
            await _metadataRepository.DeleteAsync(id);
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
    }
}
