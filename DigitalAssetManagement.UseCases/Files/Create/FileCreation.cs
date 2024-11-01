namespace DigitalAssetManagement.UseCases.Files.Create
{
    public interface FileCreation
    {
        Task<int> AddFileMetadataAsync(FileCreationRequest request);
        Task ProcessFileChunkUploadAsync(FileChunkUploadRequest request);
    }
}
