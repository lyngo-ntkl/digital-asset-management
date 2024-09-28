using DigitalAssetManagement.Application.Dtos.Requests;

namespace DigitalAssetManagement.Application.Services
{
    public interface FileService
    {
        Task AddFiles(MultipleFilesUploadRequestDto request);
        Task AddFilePermission(int fileId, PermissionRequestDto request);
        Task DeleteFile(int fileId);
    }
}
