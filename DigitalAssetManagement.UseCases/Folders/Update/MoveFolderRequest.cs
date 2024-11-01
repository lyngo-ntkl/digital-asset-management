namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class MoveFolderRequest
    {
        public int FolderId { get; set; }
        public int NewParentId { get; set; }
    }
}
