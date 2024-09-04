using DigitalAssetManagement.Application.Dtos.Responses;

namespace DigitalAssetManagement.Application.Services
{
    public interface DriveService
    {
        Task<List<DriveResponseDto>> GetDriveOwnedByLoginUser(string? name);
    }
}
