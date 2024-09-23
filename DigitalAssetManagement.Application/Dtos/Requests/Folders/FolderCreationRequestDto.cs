using DigitalAssetManagement.Application.Common.Attributes;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    public class FolderCreationRequestDto
    {
        public required string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
