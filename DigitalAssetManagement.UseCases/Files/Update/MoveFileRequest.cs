namespace DigitalAssetManagement.UseCases.Files.Update
{
    public class MoveFileRequest
    {
        public required int FileId { get; set; }
        public required int NewParentId { get; set; }
    }
}
