using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Services
{
    public interface MetadataService
    {
        Task<Metadata> Add(Metadata metadata);
        Task<Metadata> Add(string name, string absolutePath, int ownerId, MetadataType type);
        Task<Metadata> AddDrive(string name, string absolutePath, int ownerId);
        Task<Metadata> GetById(int id);
        Task<Metadata?> GetLoginUserDriveMetadata();
        //Task<DriveDetailsResponseDto> Create(DriveRequestDto request);
        //Task Delete(int id);
        //Task<DriveDetailsResponseDto> GetById(int id);
        //Task<List<DriveResponseDto>> GetDriveOwnedByLoginUser(string? name);
        //Task MoveToTrash(int id);
        //Task<DriveDetailsResponseDto> Update(int id, DriveRequestDto request);
    }
}
