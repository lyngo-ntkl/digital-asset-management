using DigitalAssetManagement.Application.Common.Attributes;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    [Xor(nameof(ParentDriveId), nameof(ParentFolderId))]
    public class FolderCreationRequestDto
    {
        public required string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }
    }
}
