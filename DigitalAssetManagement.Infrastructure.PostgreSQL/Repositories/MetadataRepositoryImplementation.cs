using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories
{
    public class MetadataRepositoryImplementation(ApplicationDbContext context, IMapper mapper): IMetadataRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Entities.DomainEntities.Metadata> AddAsync(Entities.DomainEntities.Metadata metadata)
        {
            var dbMetadata = await _context.Metadata.AddAsync(_mapper.Map<Metadata>(metadata));
            await _context.SaveChangesAsync();
            return _mapper.Map<Entities.DomainEntities.Metadata>(dbMetadata.Entity);
        }

        public async Task DeleteAsync(int id)
        {
            var metadata = await _context.Metadata.FindAsync(id);
            if (metadata != null)
            {
                _context.Metadata.Remove(metadata);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistByIdAndTypeAsync(int id, MetadataType type)
        {
            return await _context.Metadata.AnyAsync(m => m.Id == id && m.Type == type);
        }

        public async Task<Entities.DomainEntities.Metadata> GetByIdAsync(int id)
        {
            var dbMetadata = await _context.Metadata.FirstOrDefaultAsync(m => m.Id == id);

            if (dbMetadata == null)
            {
                throw new Exception();
            }

            return _mapper.Map<Entities.DomainEntities.Metadata>(dbMetadata);
        }

        public async Task<Entities.DomainEntities.Metadata> GetByIdAndTypeAsync(int id, MetadataType type)
        {
            var metadata = await _context.Metadata.FirstOrDefaultAsync(m => m.Id == id && m.Type == type);
            return _mapper.Map<Entities.DomainEntities.Metadata>(metadata);
        }

        public ICollection<Entities.DomainEntities.Metadata> GetByParentIdAndNotIsDeleted(int parentId)
        {
            var metadata = _context.Metadata.Where(m => m.ParentId == parentId && !m.IsDeleted);
            return _mapper.Map<ICollection<Entities.DomainEntities.Metadata>>(metadata);
        }

        public async Task<Entities.DomainEntities.Metadata> GetByUserIdAndTypeDrive(int userId)
        {
            var dbDrive = await _context.Metadata.FirstOrDefaultAsync(m => m.OwnerId == userId && m.Type == MetadataType.Drive);
            if (dbDrive == null)
            {
                throw new DriveNotFoundException();
            }
            return _mapper.Map<Entities.DomainEntities.Metadata>(dbDrive);
        }

        public async Task<ICollection<int>> GetMetadataIdByParentIdAsync(int parentId)
        {
            var metadataIds = _context.Metadata
                .Where(m => m.ParentId == parentId)
                .Select(m => m.Id);
            return await metadataIds.ToListAsync();
        }

        public async Task UpdateAbsolutePathByIdsAsync(ICollection<int> ids, string newParentAbsolutePath)
        {
            IQueryable<Metadata> metadata = _context.Metadata.Where(m => ids.Contains(m.Id));

            await metadata.ExecuteUpdateAsync(
                m => m.SetProperty(
                    x => x.AbsolutePath,
                    x => $"{newParentAbsolutePath}/{x.Name}"
                )
            );
        }

        public async Task UpdateAbsolutePathByParentIdAsync(int parentId, string newParentAbsolutePath)
        {
            IQueryable<Metadata> metadata = _context.Metadata.Where(m => m.ParentId == parentId);

            await metadata.ExecuteUpdateAsync(
                m => m.SetProperty(
                    x => x.AbsolutePath,
                    x => $"{newParentAbsolutePath}/{x.Name}"
                )
            );
        }

        public async Task UpdateAsync(Entities.DomainEntities.Metadata permission)
        {
            var dbMetadata = await _context.Metadata.FindAsync(permission.Id);
            if (dbMetadata != null)
            {
                dbMetadata = _mapper.Map(permission, dbMetadata);
                _context.Metadata.Update(dbMetadata);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIsDeletedByIdAsync(int id)
        {
            var metadata = await _context.Metadata.FindAsync(id);
            metadata.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIsDeletedByParentIdAsync(int parentId)
        {
            IQueryable<Metadata> metadata = _context.Metadata.Where(m => m.ParentId == parentId);
            await metadata.ExecuteUpdateAsync(x => x.SetProperty(m => m.IsDeleted, m => true));
        }
    }
}
