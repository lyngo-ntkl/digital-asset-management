using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class PermissionRepositoryImplementation : GenericRepositoryImplementation<Permission>, PermissionRepository
    {
        public PermissionRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
