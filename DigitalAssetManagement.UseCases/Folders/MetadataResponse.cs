namespace DigitalAssetManagement.UseCases.Folders
{
    public class MetadataResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string MetadataType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
