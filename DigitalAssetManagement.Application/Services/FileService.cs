using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task<FileResponseDto> GetFile(int fileId);
    }
}
