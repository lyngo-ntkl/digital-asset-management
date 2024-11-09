using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using DigitalAssetManagement.UseCases.Common;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories
{
    public class UserRepositoryImplementation(ApplicationDbContext context, IMapper mapper) : IUserRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Entities.DomainEntities.User> AddAsync(Entities.DomainEntities.User user)
        {
            var dbUser = await _context.Users.AddAsync(_mapper.Map<User>(user));
            await _context.SaveChangesAsync();
            return _mapper.Map<Entities.DomainEntities.User>(dbUser.Entity);
        }

        public async Task DeleteAsync(Entities.DomainEntities.User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser != null)
            {
                _context.Users.Remove(dbUser);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistByIdAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<ICollection<Entities.DomainEntities.User>> GetByContainingEmailWithPaginationAsync(string email, int pageSize = 10, int page = 1)
        {
            var users = _context.Users
                .Where(u => u.Email.Contains(email))
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            return _mapper.Map<ICollection<Entities.DomainEntities.User>>(await users.ToListAsync());
        }

        public async Task<Entities.DomainEntities.User?> GetByEmailAsync(string email)
        {
            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return _mapper.Map<Entities.DomainEntities.User?>(dbUser);
        }

        public async Task<Entities.DomainEntities.User?> GetByIdAsync(int id)
        {
            var dbUser = await _context.Users.FindAsync(id);
            return _mapper.Map<Entities.DomainEntities.User?>(dbUser);
        }

        public async Task UpdateAsync(Entities.DomainEntities.User user)
        {
            var dbUser = await _context.Users.FindAsync(user.Id);

            if (dbUser != null)
            {
                dbUser = _mapper.Map(user, dbUser);
                _context.Users.Update(dbUser);
            }
            await _context.SaveChangesAsync();
        }
    }
}
