using DigitalAssetManagement.Application.Dtos.Requests;
using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task AddFiles(MultipleFilesUploadRequestDto request);
        Task<int> AddFileMetadataAsync(FileMetadataCreationRequestDto request);
        Task AddFilePermission(int fileId, PermissionCreationRequest request);
        Task DeleteFile(int fileId);
        Task DeleteFileSoftly(int fileId);
        Task<FileResponseDto> GetFile(int fileId);
    }
}
