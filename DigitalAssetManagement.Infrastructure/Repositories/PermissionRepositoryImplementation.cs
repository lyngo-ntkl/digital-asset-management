using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Repositories;
using AutoMapper;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class PermissionRepositoryImplementation(ApplicationDbContext context, IMapper mapper) : PermissionRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Entities.DomainEntities.Permission> AddAsync(Entities.DomainEntities.Permission permission)
        {
            var dbPermission = await _context.Permissions.AddAsync(_mapper.Map<Permission>(permission));
            return _mapper.Map<Entities.DomainEntities.Permission>(dbPermission.Entity);
        }

        public async Task AddRangeAsync(IEnumerable<Entities.DomainEntities.Permission> permissions)
        {
            var dbPermissions = _mapper.Map<IEnumerable<Permission>>(permissions);
            await _context.Permissions.AddRangeAsync(dbPermissions);
        }

        public void Delete(Entities.DomainEntities.Permission permission) => _context.Permissions.Remove(permission);

        public async Task DeleteRangeAsync(Expression<Func<Permission, bool>>? filter = null)
        {
            IQueryable<Permission> permissions = _context.Permissions;
            if (filter != null)
            {
                permissions = permissions.Where(filter);
            }
            await permissions.ExecuteDeleteAsync();
        }

        public async Task<ICollection<Entities.DomainEntities.Permission>> GetAllAsync(
            Expression<Func<Entities.DomainEntities.Permission, bool>>? filter = null,
            Func<IQueryable<Entities.DomainEntities.Permission>, IOrderedQueryable<Entities.DomainEntities.Permission>>? orderedQuery = null,
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

        public ICollection<Entities.DomainEntities.Permission> GetByMetadataIdAndNotIsDeletedNoTrackingAsync(int metadataId)
        {
            var dbPermissions = _context.Permissions
                .Where(p => p.MetadataId == metadataId && !p.IsDeleted)
                .AsNoTracking();
            return _mapper.Map<ICollection<Entities.DomainEntities.Permission>>(dbPermissions);
        }

        public async Task<Permission?> GetByUserIdAndMetadataIdAsync(int userId, int metadataId)
        {
            return await _context.Permissions.FirstOrDefaultAsync(
                p => p.UserId == userId && p.MetadataId == metadataId && !p.IsDeleted
            );
        }

        public async Task<ICollection<Permission>> GetByUserIdAndMetadataIdsAsync(int userId, IEnumerable<int> metadataIds)
        {
            var permissions = _context.Permissions
                .Where(p => p.UserId == userId && metadataIds.Contains(p.MetadataId));
            return await permissions.ToListAsync();
        }

        public async Task<Role?> GetRoleByUserIdAndMetadataId(int userId, int metadataId)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.UserId == userId && p.MetadataId == metadataId && !p.IsDeleted);
            return permission.Role;
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
