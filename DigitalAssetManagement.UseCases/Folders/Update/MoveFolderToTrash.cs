namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public interface MoveFolderToTrash
    {
        Task MoveFolderToTrashAsync(int folderId);
    }
}
