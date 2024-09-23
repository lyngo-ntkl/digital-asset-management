using DigitalAssetManagement.Application.Common.Attributes;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;

namespace DigitalAssetManagement.Application.Dtos.Responses.Folders
{
    public class FolderDetailResponseDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string AbsolutePath { get; set; }
        public ICollection<FolderResponseDto>? SubFolders { get; set; }
        public ICollection<FileResponseDto>? Files { get; set; }
        public ICollection<PermissionResponseDto>? Permissions { get; set; }
        public bool IsDeleted { get; set; }
    }
}
