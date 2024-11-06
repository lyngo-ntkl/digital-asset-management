﻿using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Entities.Enums;
using System.Linq.Expressions;

namespace DigitalAssetManagement.UseCases.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> AddAsync(Permission permission);
        Task AddRangeAsync(IEnumerable<Permission> permissions);
        Task DeleteAsync(Permission permission);
        Task DeleteByMetadataIdsAsync(ICollection<int> ids);
        Task DeleteByMetadataId(int metadataId);
        ICollection<Permission> GetByMetadataIdNoTracking(int metadataId);
        Task<Permission?> GetByUserIdAndMetadataIdAsync(int userId, int metadataId);
        Task<ICollection<Permission>> GetByUserIdAndMetadataIdsAsync(int userId, IEnumerable<int> metadataIds);
        Task<Role?> GetRoleByUserIdAndMetadataId(int userId, int metadataId);
        IEnumerable<int> GetUserIdByMetadataId(int metadataId);
        Task UpdateAsync(Permission permission);
        Task UpdateRoleByIdsAsync(Role role, IEnumerable<int> ids);
    }
}