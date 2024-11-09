using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;

namespace DigitalAssetManagement.UseCases.Repositories
{
    public interface IMetadataRepository
    {
        Task<Metadata> AddAsync(Metadata metadata);
        Task DeleteAsync(int id);
        Task<bool> ExistByIdAndTypeAsync(int id, MetadataType type);
        Task<Metadata> GetByIdAsync(int id);
        Task<Metadata> GetByIdAndTypeAsync(int id, MetadataType type);
        ICollection<Metadata> GetByParentIdAndNotIsDeleted(int parentId);
        Task<Metadata> GetByUserIdAndTypeDrive(int userId);
        Task<ICollection<int>> GetMetadataIdByParentIdAsync(int parentId);
        Task UpdateAbsolutePathByIdsAsync(ICollection<int> ids, string newParentAbsolutePath);
        Task UpdateAbsolutePathByParentIdAsync(int parentId, string newParentAbsolutePath);
        Task UpdateAsync(Metadata permission);
        Task UpdateIsDeletedByIdAsync(int id);
        Task UpdateIsDeletedByParentIdAsync(int parentId);
    }
}
