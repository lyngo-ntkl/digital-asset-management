using DigitalAssetManagement.Application.Common.Requests;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    public class FolderCreationRequestDto: MetadataParentRequestDto
    {
        public required string Name { get; set; }
    }
}
