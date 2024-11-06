using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Common.Exceptions;

namespace DigitalAssetManagement.UseCases.Folders.Delete
{
    public class FolderDeletionHandler(IMetadataRepository metadataRepository, ISystemFolderHelper systemFolderHelper): FolderDeletion
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly ISystemFolderHelper _systemFolderHelper = systemFolderHelper;

        public async Task DeleteFolderAsync(int id)
        {
            Metadata metadata = await GetFolderAsync(id);
            await _metadataRepository.DeleteAsync(id);
            _systemFolderHelper.DeleteFolder(metadata.AbsolutePath);
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
