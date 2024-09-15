using DigitalAssetManagement.Application.Dtos.Requests.Files;
using DigitalAssetManagement.Application.Dtos.Responses.Files;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task<FileResponseDto> UploadFile(FileUploadRequestDto request);
    }
}
