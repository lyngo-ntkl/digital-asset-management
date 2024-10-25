namespace DigitalAssetManagement.Application.Dtos.Requests
{
    public class FileMetadataCreationRequestDto
    {
        public required int ParentId { get; set; }
        public required string FileName { get; set; }
    }
}
