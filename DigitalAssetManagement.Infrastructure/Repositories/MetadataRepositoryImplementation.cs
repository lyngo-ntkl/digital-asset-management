using AutoMapper;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.UseCases.Repositories;

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
            var dbMetadata = await _context.Metadata.FindAsync(id);
            return _mapper.Map<Entities.DomainEntities.Metadata?>(dbMetadata);
        }
    }
}
