namespace DigitalAssetManagement.UseCases.Files.Update
{
    public interface FileSoftDeletion
    {
        Task DeleteFileSoftlyAsync(int fileId);
    }
}
