namespace DigitalAssetManagement.UseCases.Folders.Create
{
    public interface FolderCreation
    {
        Task<FolderDetailResponse> AddFolderAsync(FolderCreationRequest request);
    }
}
