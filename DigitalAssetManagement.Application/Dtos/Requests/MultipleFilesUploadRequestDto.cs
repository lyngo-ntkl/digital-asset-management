using DigitalAssetManagement.Application.Common.Requests;
using Microsoft.AspNetCore.Http;

namespace DigitalAssetManagement.Application.Dtos.Requests
{
    public class MultipleFilesUploadRequestDto: ResourceBasedPermissionCheckingRequestDto
    {
        public required IFormFileCollection Files { get; set; }
    }
}
