using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class DriveRepositoryImplementation : GenericRepositoryImplementation<Drive>, DriveRepository
    {
        public DriveRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
