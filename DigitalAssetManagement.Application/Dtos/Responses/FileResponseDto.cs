namespace DigitalAssetManagement.Application.Dtos.Responses
{
    public class FileResponseDto
    {
        public required int Id { get; set; }
        public required string FileName { get; set; }
        // TODO: recheck this
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }
    }
}
