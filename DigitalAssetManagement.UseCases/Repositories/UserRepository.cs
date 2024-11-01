using DigitalAssetManagement.Entities.DomainEntities;

namespace DigitalAssetManagement.UseCases.Repositories
{
    public interface UserRepository
    {
        Task<User> AddAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistByEmailAsync(string email);
        Task<bool> ExistByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
}
