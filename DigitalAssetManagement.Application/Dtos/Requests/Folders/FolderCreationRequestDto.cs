using DigitalAssetManagement.Application.Common.Requests;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    public class FolderCreationRequestDto
    {
        public required int ParentId { get; set; }
        public required string Name { get; set; }
    }
}
