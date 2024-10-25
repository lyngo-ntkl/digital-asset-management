using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class PermissionRepositoryImplementation(ApplicationDbContext context) : PermissionRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Permission> AddAsync(Permission permission)
        {
            var dbPermission = await _context.Permissions.AddAsync(permission);
            return dbPermission.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<Permission> permissions) => await _context.Permissions.AddRangeAsync(permissions);

        public void Delete(Permission permission) => _context.Permissions.Remove(permission);

        public async Task DeleteRangeAsync(Expression<Func<Permission, bool>>? filter = null)
        {
            IQueryable<Permission> permissions = _context.Permissions;
            if (filter != null)
            {
                permissions = permissions.Where(filter);
            }
            await permissions.ExecuteDeleteAsync();
        }

        public async Task<ICollection<Permission>> GetAllAsync(
            Expression<Func<Permission, bool>>? filter = null,
            Func<IQueryable<Permission>, IOrderedQueryable<Permission>>? orderedQuery = null,
            string includedProperties = "",
            bool isTracked = true,
            bool isPaging = false,
            int pageSize = 10,
            int page = 1
        )
        {
            IQueryable<Permission> permissions = _context.Permissions;

            if (filter != null)
            {
                permissions = permissions.Where(filter);
            }

            if (orderedQuery != null)
            {
                permissions = orderedQuery(permissions);
            }

            foreach (string includedProperty in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                permissions = permissions.Include(includedProperty);
            }

            if (!isTracked)
            {
                permissions = permissions.AsNoTracking();
            }

            if (isPaging)
            {
                permissions.Skip((page - 1) * pageSize)
                    .Take(pageSize);
            }

            return await permissions.ToListAsync();
        }

        public async Task<Permission?> GetByUserIdAndMetadataIdAsync(int userId, int metadataId)
        {
            return await _context.Permissions.FirstOrDefaultAsync(
                p => p.UserId == userId && p.MetadataId == metadataId && !p.IsDeleted
            );
        }

        public IEnumerable<int> GetUserIdByMetadataId(int metadataId)
        {
            return _context.Permissions
                .Where(p => p.MetadataId == metadataId)
                .Select(p => p.UserId);
        }

        public void Update(Permission permission) => _context.Permissions.Update(permission);

        public async Task<int> UpdateRangeAsync(Expression<Func<SetPropertyCalls<Permission>, SetPropertyCalls<Permission>>> updatedProperties, Expression<Func<Permission, bool>> filter)
        {
            IQueryable<Permission> permissions = _context.Permissions;
            if (filter != null)
            {
                permissions = permissions.Where(filter);
            }
            return await permissions.ExecuteUpdateAsync(updatedProperties);
        }
    }
}
