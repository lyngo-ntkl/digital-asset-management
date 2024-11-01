namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public interface GetDrive
    {
        Task<FolderDetailResponse> GetDrive();
    }
}
