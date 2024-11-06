namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public interface FolderNameModification
    {
        Task<FolderDetailResponse> RenameFolderAsync(MetadataNameModificationRequest request);
    }
}
