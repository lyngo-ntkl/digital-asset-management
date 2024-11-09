namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public interface IGetDrive
    {
        Task<FolderDetailResponse> GetDrive();
    }
}
