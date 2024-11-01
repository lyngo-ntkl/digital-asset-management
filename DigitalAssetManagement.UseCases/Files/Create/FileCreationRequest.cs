namespace DigitalAssetManagement.UseCases.Files.Create
{
    public class FileCreationRequest
    {
        public required int ParentId { get; set; }
        public required string FileName { get; set; }
    }
}
