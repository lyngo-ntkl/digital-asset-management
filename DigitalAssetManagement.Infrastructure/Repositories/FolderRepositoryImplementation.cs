using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class FolderRepositoryImplementation : GenericRepositoryImplementation<Folder>, FolderRepository
    {
        public FolderRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
