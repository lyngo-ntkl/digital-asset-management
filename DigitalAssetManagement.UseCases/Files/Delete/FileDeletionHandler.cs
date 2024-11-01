using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Files.Delete
{
    public class FileDeletionHandler(MetadataRepository metadataRepository, SystemFileHelper systemFileHelper): FileDeletion
    {
        private readonly MetadataRepository _metadataRepository = metadataRepository;
        private readonly SystemFileHelper _systemFileHelper = systemFileHelper;
        public async Task DeleteFileAsync(int fileId)
        {
            var deletedFileMetadata = await _metadataRepository.GetByIdAsync(fileId);
            if (deletedFileMetadata == null || deletedFileMetadata.Type != Entities.Enums.MetadataType.File)
            {
                throw new Exception();
            }
            _systemFileHelper.DeleteFile(deletedFileMetadata.AbsolutePath);
            await _metadataRepository.DeleteAsync(fileId);
        }
    }
}
