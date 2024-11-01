namespace DigitalAssetManagement.UseCases.Folders.Create
{
    public class FolderCreationRequest
    {
        public required int ParentId { get; set; }
        public required string Name { get; set; }
    }
}
