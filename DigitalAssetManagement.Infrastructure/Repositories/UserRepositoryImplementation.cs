using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class UserRepositoryImplementation : GenericRepositoryImplementation<User>, UserRepository
    {
        public UserRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }
    }
}
