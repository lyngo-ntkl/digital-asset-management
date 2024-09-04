using DigitalAssetManagement.Application.Common.Attributes;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;

namespace DigitalAssetManagement.Application.Dtos.Responses.Folders
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
