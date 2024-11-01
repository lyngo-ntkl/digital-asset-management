namespace DigitalAssetManagement.UseCases.Files.Delete
{
    public interface FileDeletion
    {
        Task DeleteFileAsync(int fileId);
    }
}
