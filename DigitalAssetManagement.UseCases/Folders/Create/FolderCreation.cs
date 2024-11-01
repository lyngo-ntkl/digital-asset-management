namespace DigitalAssetManagement.UseCases.Folders.Create
{
    public interface FolderCreation
    {
        Task<FolderDetailResponse> AddFolder(FolderCreationRequest request);
    }
}
