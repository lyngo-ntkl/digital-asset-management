
namespace DigitalAssetManagement.Application.Services
{
    public interface UserService
    {
        Task<User?> GetById(int id);
        Task<User> GetByEmail(string email);
    }
}
