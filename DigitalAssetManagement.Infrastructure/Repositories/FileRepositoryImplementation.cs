using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class FileRepositoryImplementation : GenericRepositoryImplementation<DigitalAssetManagement.Domain.Entities.File>, FileRepository
    {
        public FileRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
