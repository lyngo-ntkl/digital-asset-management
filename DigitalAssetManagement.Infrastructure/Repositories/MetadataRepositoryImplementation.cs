using AutoMapper;
using DigitalAssetManagement.Entities.Enums;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class MetadataRepositoryImplementation(ApplicationDbContext context, IMapper mapper): MetadataRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Entities.DomainEntities.Metadata> AddAsync(Entities.DomainEntities.Metadata metadata)
        {
            var dbMetadata = await _context.Metadata.AddAsync(_mapper.Map<Metadata>(metadata));
            await _context.SaveChangesAsync();
            return _mapper.Map<Entities.DomainEntities.Metadata>(dbMetadata);
        }

        public async Task<Entities.DomainEntities.Metadata?> GetByIdAsync(int id)
        {
            var dbMetadata = await _context.Metadata
                .Include(m => m.Permissions)
                .Include(m => m.Children)
                .FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map<Entities.DomainEntities.Metadata?>(dbMetadata);
        }

        public async Task<Entities.DomainEntities.Metadata> GetByUserIdAndTypeDrive(int userId)
        {
            var dbDrive = await _context.Metadata.FirstOrDefaultAsync(m => m.OwnerId == userId && m.MetadataType == MetadataType.Drive);
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

        public async Task UpdateAsync(Entities.DomainEntities.Metadata permission)
        {
            var dbMetadata = await _context.Metadata.FindAsync(permission.Id);
            dbMetadata = _mapper.Map(permission, dbMetadata);
            _context.Metadata.Update(dbMetadata);
            await _context.SaveChangesAsync();
        }
    }
}
