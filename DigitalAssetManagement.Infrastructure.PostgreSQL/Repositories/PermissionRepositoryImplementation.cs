using Microsoft.EntityFrameworkCore;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories
{
    public class PermissionRepositoryImplementation(ApplicationDbContext context, IMapper mapper) : IPermissionRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Entities.DomainEntities.Permission> AddAsync(Entities.DomainEntities.Permission permission)
        {
            var dbPermission = await _context.Permissions.AddAsync(_mapper.Map<Permission>(permission));
            await _context.SaveChangesAsync();
            return _mapper.Map<Entities.DomainEntities.Permission>(dbPermission.Entity);
        }

        public async Task AddRangeAsync(IEnumerable<Entities.DomainEntities.Permission> permissions)
        {
            var dbPermissions = _mapper.Map<IEnumerable<Permission>>(permissions);
            await _context.Permissions.AddRangeAsync(dbPermissions);
        }

        public async Task DeleteAsync(Entities.DomainEntities.Permission permission)
        {
            var dbPermission = await _context.Permissions.FindAsync(permission.Id);
            _context.Permissions.Remove(dbPermission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByMetadataIdsAsync(ICollection<int> ids)
        {
            IQueryable<Permission> permissions = _context.Permissions.Where(p => ids.Contains(p.MetadataId));
            await permissions.ExecuteDeleteAsync();
        }

        public async Task DeleteByMetadataId(int metadataId)
        {
            IQueryable<Permission> permissions = _context.Permissions.Where(m => m.MetadataId == metadataId);
            await permissions.ExecuteDeleteAsync();
        }

        public ICollection<Entities.DomainEntities.Permission> GetByMetadataIdNoTracking(int metadataId)
        {
            var dbPermissions = _context.Permissions
                .Where(p => p.MetadataId == metadataId)
                .AsNoTracking();
            return _mapper.Map<ICollection<Entities.DomainEntities.Permission>>(dbPermissions);
        }

        public async Task<Entities.DomainEntities.Permission?> GetByUserIdAndMetadataIdAsync(int userId, int metadataId)
        {
            var dbPermissions = await _context.Permissions.FirstOrDefaultAsync(
                p => p.UserId == userId && p.MetadataId == metadataId
            );
            return _mapper.Map<Entities.DomainEntities.Permission>(dbPermissions);
        }

        public async Task<ICollection<Entities.DomainEntities.Permission>> GetByUserIdAndMetadataIdsAsync(int userId, IEnumerable<int> metadataIds)
        {
            var permissions = _context.Permissions
                .Where(p => p.UserId == userId && metadataIds.Contains(p.MetadataId));
            return _mapper.Map<ICollection<Entities.DomainEntities.Permission>>(await permissions.ToListAsync());
        }

        public async Task<Role?> GetRoleByUserIdAndMetadataId(int userId, int metadataId)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p => p.UserId == userId && p.MetadataId == metadataId);
            return permission.Role;
        }

        public IEnumerable<int> GetUserIdByMetadataId(int metadataId)
        {
            return _context.Permissions
                .Where(p => p.MetadataId == metadataId)
                .Select(p => p.UserId);
        }

        public async Task UpdateAsync(Entities.DomainEntities.Permission permission)
        {
            var dbPermission = await _context.Permissions.FindAsync(permission.Id);
            if (dbPermission != null)
            {
                dbPermission = _mapper.Map(permission, dbPermission);
                _context.Permissions.Update(dbPermission);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleByIdsAsync(Role role, IEnumerable<int> ids)
        {
            IQueryable<Permission> permissions = _context.Permissions.Where(p => ids.Contains(p.Id));
            await permissions.ExecuteUpdateAsync(
                x => x.SetProperty(p => p.Role, role));
        }
    }
}
