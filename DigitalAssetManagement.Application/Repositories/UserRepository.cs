using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Application.Repositories
{
    public interface UserRepository
    {
        Task<User> AddAsync(User user);
        void Delete(User user);
        Task<bool> ExistByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        void Update(User user);
    }
}
