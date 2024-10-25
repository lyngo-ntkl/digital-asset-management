using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class UserRepositoryImplementation(ApplicationDbContext context) : UserRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<User> AddAsync(User user)
        {
            var dbUser = await _context.Users.AddAsync(user);
            return dbUser.Entity;
        }

        public void Delete(User user)
        {
            if (_context.Users.Entry(user).State == EntityState.Detached)
            {
                _context.Users.Attach(user);
            }

            _context.Users.Entry(user).State = EntityState.Deleted;
        }

        public async Task<bool> ExistByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public void Update(User user)
        {
            if (_context.Users.Entry(user).State == EntityState.Detached)
            {
                _context.Users.Attach(user);
            }

            _context.Users.Entry(user).State = EntityState.Modified;
        }
    }
}
