using DigitalAssetManagement.Entities.DomainEntities;
using DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DigitalAssetManagement.Infrastructure.Redis.Repositories
{
    public class CachedUserRepositoryDecorator : IUserRepository
    {
        private static readonly JsonSerializerSettings serializerSettings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        private readonly UserRepositoryImplementation _userRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;

        public CachedUserRepositoryDecorator(
            UserRepositoryImplementation implementation,
            IDistributedCache distributedCache,
            IConfiguration configuration)
        {
            _userRepository = implementation;
            _distributedCache = distributedCache;
            _configuration = configuration;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(double.Parse(_configuration["jwt:expirationDays"]!))
            };
        }

        public Task<User> AddAsync(User entity)
        {
            return _userRepository.AddAsync(entity);
        }

        public async Task<bool> ExistByEmailAsync(string email)
        {
            return await _userRepository.ExistByEmailAsync(email);
        }

        public async Task<bool> ExistByIdAsync(int id)
        {
            return await _userRepository.ExistByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task DeleteAsync(User entity)
        {
            await _userRepository.DeleteAsync(entity);
        }

        public async Task<ICollection<User>> GetByContainingEmailWithPaginationAsync(string email, int pageSize = 10, int page = 1)
        {
            var users = await _userRepository.GetByContainingEmailWithPaginationAsync(email, pageSize, page);
            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            string key = $"user{id}";
            var cachedUser = await _distributedCache.GetStringAsync(key);

            User? user;
            if (cachedUser == null)
            {
                user = await _userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings), _options);
                }
                return user;
            }

            user = JsonConvert.DeserializeObject<User>(cachedUser, serializerSettings);
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);

            string key = $"user{user.Id}";
            if (await _distributedCache.GetAsync(key) != null)
            {
                await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings), _options);
            }
        }
    }
}
