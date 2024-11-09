namespace DigitalAssetManagement.UseCases.Folders
{
    public class MetadataResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string MetadataType { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required DateTime LastModifiedDate { get; set; }
        public string? ContentType { get; set; }
    }
}
