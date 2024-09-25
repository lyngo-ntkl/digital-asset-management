namespace DigitalAssetManagement.Application.Dtos.Responses.Folders
{
    public class FolderResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string MetadataType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
