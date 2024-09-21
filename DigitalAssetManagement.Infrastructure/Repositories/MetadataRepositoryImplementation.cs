using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class MetadataRepositoryImplementation : GenericRepositoryImplementation<Metadata>, MetadataRepository
    {
        public MetadataRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
