using DigitalAssetManagement.Application.Common.Attributes;

namespace DigitalAssetManagement.Application.Dtos.Responses
{
    [Xor(nameof(ParentFolder), nameof(ParentDrive))]
    public class FolderDetailResponseDto
    {
        public required int Id { get; set; }
        public required string FolderName { get; set; }
        public FolderResponseDto? ParentFolder { get; set; }
        public DriveResponseDto? ParentDrive { get; set; }
        public ICollection<FolderResponseDto>? SubFolders { get; set; }
        public ICollection<FileResponseDto>? Files { get; set; }
        public ICollection<PermissionResponseDto>? Permissions { get; set; }
    }
}
