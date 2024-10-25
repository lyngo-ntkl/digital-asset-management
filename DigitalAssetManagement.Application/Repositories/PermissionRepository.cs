using DigitalAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface PermissionRepository
    {
        Task<Permission> AddAsync(Permission permission);
        Task AddRangeAsync(IEnumerable<Permission> permissions);
        void Delete(Permission permission);
        Task DeleteRangeAsync(Expression<Func<Permission, bool>> filter);
        Task<ICollection<Permission>> GetAllAsync(
            Expression<Func<Permission, bool>>? filter = null, 
            Func<IQueryable<Permission>, IOrderedQueryable<Permission>>? orderedQuery = null, 
            string includedProperties = "", 
            bool isTracked = true, 
            bool isPaging = false,
            int pageSize = 10, 
            int page = 1
        );
        Task<Permission?> GetByUserIdAndMetadataIdAsync(int userId, int metadataId);
        IEnumerable<int> GetUserIdByMetadataId(int metadataId);
        void Update(Permission permission);
        Task<int> UpdateRangeAsync(Expression<Func<SetPropertyCalls<Permission>, SetPropertyCalls<Permission>>> updatedProperties, Expression<Func<Permission, bool>> filter);
    }
}
