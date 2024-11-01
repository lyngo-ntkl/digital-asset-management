namespace DigitalAssetManagement.UseCases.Folders.Update
{
    public class MetadataNameModificationRequest
    {
        public required int Id { get; set; }
        public required string NewName { get; set; }
    }
}
