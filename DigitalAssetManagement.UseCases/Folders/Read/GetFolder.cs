namespace DigitalAssetManagement.UseCases.Folders.Read
{
    public interface GetFolder
    {
        Task<FolderDetailResponse> GetFolder(int id);
    }
}
