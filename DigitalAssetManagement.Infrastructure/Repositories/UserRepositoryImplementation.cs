using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class UserRepositoryImplementation : GenericRepositoryImplementation<User>, UserRepository
    {
        public UserRepositoryImplementation(ApplicationDbContext context) : base(context)
        {
        }

        public bool ExistByEmail(string email)
        {
            return _dbSet.Any(x => x.Email == email);
        }

        public User? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(user => user.Email == email);
        }
    }
}
