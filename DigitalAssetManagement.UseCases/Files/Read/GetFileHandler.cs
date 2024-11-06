using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Common.Exceptions;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.UseCases.Files.Read
{
    public class GetFileHandler(IMetadataRepository metadataRepository, ISystemFileHelper systemFileHelper) : GetFile
    {
        private readonly IMetadataRepository _metadataRepository = metadataRepository;
        private readonly ISystemFileHelper _systemFileHelper = systemFileHelper;

        public async Task<FileContentResponse> GetFileContentAsync(int fileId)
        {
            var file = await _metadataRepository.GetByIdAsync(fileId);
            if (file == null || file.Type != Entities.Enums.MetadataType.File)
            {
                throw new NotFoundException();
            }

            var fileBytes = await _systemFileHelper.GetFile(file.AbsolutePath);
            return new FileContentResponse
            {
                FileContent = fileBytes,
                FileName = file.Name
            };
        }

        public Task GetFileMetadataAsync(int fileId)
        {
            throw new NotImplementedException();
        }
    }
}
