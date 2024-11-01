using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Repositories;
using AutoMapper;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories
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
