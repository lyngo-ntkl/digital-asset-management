using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface UserRepository: GenericRepository<User>
    {
        bool ExistByEmail(string email);
        User? GetByEmail(string email);
        Task<User?> GetByEmailAsync(string email);
    }
}
