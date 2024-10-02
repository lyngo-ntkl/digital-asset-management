using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Domain.Enums;

namespace DigitalAssetManagement.Application.Services
{
    public interface MetadataService
    {
        Task<Metadata> Add(Metadata metadata);
        Task<Metadata> Add(string name, string absolutePath, int ownerId, MetadataType type);
        Task<Metadata> AddDrive(string name, string absolutePath, int ownerId);
        Task AddRange(ICollection<Metadata> metadata);
        Task DeleteMetadata(Metadata metadata);
        Task<Metadata> GetById(int id);
        Task<ICollection<Metadata>> GetByAbsolutePathStartsWith(string absolutePath);
        Task<Metadata> GetFileMetadataById(int id);
        Task<Metadata> GetFolderMetadataById(int id);
        Task<Metadata> GetFolderOrDriveMetadataById(int id);
        Task<Metadata?> GetLoginUserDriveMetadata();
        Task<bool> IsFileExist(int id);
        Task<bool> IsFolderExist(int id);
        Task Update(Metadata metadata);
        Task<int> UpdateFolderAbsolutePathAsync(string oldFolderAbsolutePath, string newFolderAbsolutePath);
    }
}
