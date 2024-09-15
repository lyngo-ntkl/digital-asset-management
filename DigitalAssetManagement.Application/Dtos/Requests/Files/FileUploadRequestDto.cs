using DigitalAssetManagement.Application.Common.Attributes;
using Microsoft.AspNetCore.Http;

namespace DigitalAssetManagement.Application.Dtos.Requests.Files
{
    [Xor(nameof(FolderId), nameof(DriveId))]
    public class FileUploadRequestDto
    {
        public required IFormFile File {  get; set; }
        public int? FolderId { get; set; }
        public int? DriveId { get; set; }
    }
}
