using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Files.Delete
{
    public class FileDeletionHandler(IMetadataRepository metadataRepository, ISystemFileHelper systemFileHelper): FileDeletion
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly ISystemFileHelper _systemFileHelper = systemFileHelper;
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
