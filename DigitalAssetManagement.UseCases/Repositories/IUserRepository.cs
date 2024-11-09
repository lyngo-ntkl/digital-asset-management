using DigitalAssetManagement.Entities.DomainEntities;

namespace DigitalAssetManagement.UseCases.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistByEmailAsync(string email);
        Task<bool> ExistByIdAsync(int id);
        Task<ICollection<User>> GetByContainingEmailWithPaginationAsync(string email, int pageSize = 10, int page = 1);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
}
