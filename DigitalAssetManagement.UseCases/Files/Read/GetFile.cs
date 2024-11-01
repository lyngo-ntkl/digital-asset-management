namespace DigitalAssetManagement.UseCases.Files.Read
{
    public interface GetFile
    {
        Task<FileContentResponse> GetFileContentAsync(int fileId);
        Task GetFileMetadataAsync(int fileId);
    }
}
