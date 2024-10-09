using DigitalAssetManagement.Application.Common.Requests;

namespace DigitalAssetManagement.Application.Dtos.Requests.Folders
{
    public class FolderCreationRequestDto: ResourceBasedPermissionCheckingRequestDto
    {
        public required string Name { get; set; }
    }
}
