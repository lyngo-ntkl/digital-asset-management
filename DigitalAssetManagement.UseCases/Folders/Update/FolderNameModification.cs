namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public interface FolderNameModification
    {
        Task<FolderDetailResponse> UpdateName(MetadataNameModificationRequest request);
    }
}
