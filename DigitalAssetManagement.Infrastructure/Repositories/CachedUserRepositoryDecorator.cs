using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    public class CachedUserRepositoryDecorator : UserRepository
    {
        private static JsonSerializerSettings serializerSettings = new()
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

        public User Add(User entity)
        {
            return _userRepository.Add(entity);
        }

        public Task<User> AddAsync(User entity)
        {
            return _userRepository.AddAsync(entity);
        }

        public void BatchAdd(IEnumerable<User> entities)
        {
            _userRepository.BatchAdd(entities);
        }

        public async Task BatchAddAsync(IEnumerable<User> entities)
        {
            await _userRepository.BatchAddAsync(entities);
        }

        public void BatchDelete(Expression<Func<User, bool>>? filter = null)
        {
            _userRepository.BatchDelete(filter);
        }

        public async Task BatchDeleteAsync(Expression<Func<User, bool>>? filter = null)
        {
            await _userRepository.BatchDeleteAsync(filter);
        }

        public int BatchUpdate(Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls, Expression<Func<User, bool>>? filter = null)
        {
            return _userRepository.BatchUpdate(setPropertyCalls, filter);
        }

        public async Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls, Expression<Func<User, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await _userRepository.BatchUpdateAsync(setPropertyCalls, filter, cancellationToken);
        }

        public User Delete(User entity)
        {
            return _userRepository.Delete(entity);
        }

        public async Task<User?> DeleteAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public bool ExistByCondition(Expression<Func<User, bool>> condition)
        {
            return _userRepository.ExistByCondition(condition);
        }

        public async Task<bool> ExistByConditionAsync(Expression<Func<User, bool>> condition)
        {
            return await _userRepository.ExistByConditionAsync(condition);
        }

        public ICollection<User> GetAll(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            return _userRepository.GetAll(filter, orderedQuery, includedProperties, isTracked, isPaging, pageSize, page);
        }

        public IEnumerable<TProperty> GetPropertyValue<TProperty>(Expression<Func<User, TProperty>> propertySelector, Expression<Func<User, bool>>? filter = null)
        {
            return _userRepository.GetPropertyValue(propertySelector, filter);
        }

        public async Task<ICollection<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            return await _userRepository.GetAllAsync(filter, orderedQuery, includedProperties, isTracked, isPaging, pageSize, page);
        }

        public User? GetById(int id)
        {
            string key = $"user{id}";
            var cachedUser = _distributedCache.GetString(key);

            User? user;
            if (cachedUser == null)
            {
                user = _userRepository.GetById(id);
                if (user != null)
                {
                    _distributedCache.SetString(key, JsonConvert.SerializeObject(user), _options);
                }
                return user;
            }

            user = JsonConvert.DeserializeObject<User>(cachedUser);
            return user;
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

        public async Task<User?> GetByIdAsync(int id, string includedProperties)
        {
            string key = $"user{id}";
            var cachedUser = await _distributedCache.GetStringAsync(key);

            User? user;
            if (cachedUser == null)
            {
                user = await _userRepository.GetByIdAsync(id, includedProperties);
                if (user != null)
                {
                    await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings), _options);
                }
                return user;
            }

            user = JsonConvert.DeserializeObject<User>(cachedUser, serializerSettings);
            return user;
        }

        public User? GetFirstOnCondition(Func<User, bool> condition)
        {
            return _userRepository.GetFirstOnCondition(condition);
        }

        public async Task<User?> GetFirstOnConditionAsync(Expression<Func<User, bool>> condition)
        {
            return await _userRepository.GetFirstOnConditionAsync(condition);
        }

        public User Update(User entity)
        {
            return _userRepository.Update(entity);
        }
    }
}
