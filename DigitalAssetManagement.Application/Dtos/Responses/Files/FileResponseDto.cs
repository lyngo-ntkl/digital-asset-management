namespace DigitalAssetManagement.Application.Dtos.Responses.Files
{
    public class FileResponseDto
    {
        public required int Id { get; set; }
        public required string FileName { get; set; }
        // TODO: modify to file path
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
