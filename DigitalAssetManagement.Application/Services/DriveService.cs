using DigitalAssetManagement.Application.Dtos.Requests.Drives;
using DigitalAssetManagement.Application.Dtos.Responses.Drives;

namespace DigitalAssetManagement.Application.Services
{
    public interface DriveService
    {
        Task<DriveDetailsResponseDto> Create(DriveRequestDto request);
        Task<DriveDetailsResponseDto> GetById(int id);
        Task<List<DriveResponseDto>> GetDriveOwnedByLoginUser(string? name);
    }
}
