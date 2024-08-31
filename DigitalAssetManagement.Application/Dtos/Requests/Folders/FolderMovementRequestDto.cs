using DigitalAssetManagement.Application.Common.Attributes;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    [Xor(nameof(ParentDriveId), nameof(ParentFolderId))]
    public class FolderMovementRequestDto
    {
        public int? ParentFolderId { get; set; }
        public int? ParentDriveId { get; set; }
    }
}
