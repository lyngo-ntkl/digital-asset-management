namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public interface FolderSoftDeletion
    {
        Task DeleteFolderSoftly(int folderId);
    }
}
