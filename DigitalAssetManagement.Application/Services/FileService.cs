using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task DeleteFileSoftly(int fileId);
        Task<FileResponseDto> GetFile(int fileId);
    }
}
