using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace DigitalAssetManagement.Infrastructure.Repositories
{
    // TODO: should I use CQRS? cuz just cache query, not cache command
    public class CachedUserRepositoryImplementation : UserRepository
    {
        private static JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        private readonly UserRepositoryImplementation _userDecorator;
        private readonly IDistributedCache _distributedCache;

        public CachedUserRepositoryImplementation(
            UserRepositoryImplementation implementation,
            IDistributedCache distributedCache)
        {
            _userDecorator = implementation;
            _distributedCache = distributedCache;
        }

        public User Add(User entity)
        {
            return _userDecorator.Add(entity);
        }

        public Task<User> AddAsync(User entity)
        {
            return _userDecorator.AddAsync(entity);
        }

        public void BatchAdd(IEnumerable<User> entities)
        {
            _userDecorator.BatchAdd(entities);
        }

        public async Task BatchAddAsync(IEnumerable<User> entities)
        {
            await _userDecorator.BatchAddAsync(entities);
        }

        public int BatchUpdate(Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls, Expression<Func<User, bool>>? filter = null)
        {
            return _userDecorator.BatchUpdate(setPropertyCalls, filter);
        }

        public async Task<int> BatchUpdateAsync(Expression<Func<SetPropertyCalls<User>, SetPropertyCalls<User>>> setPropertyCalls, CancellationToken cancellationToken = default, Expression<Func<User, bool>>? filter = null)
        {
            return await _userDecorator.BatchUpdateAsync(setPropertyCalls, cancellationToken, filter);
        }

        public User Delete(User entity)
        {
            return _userDecorator.Delete(entity);
        }

        public async Task<User?> DeleteAsync(int id)
        {
            return await _userDecorator.DeleteAsync(id);
        }

        public bool ExistByCondition(Expression<Func<User, bool>> condition)
        {
            return _userDecorator.ExistByCondition(condition);
        }

        public async Task<bool> ExistByConditionAsync(Expression<Func<User, bool>> condition)
        {
            return await _userDecorator.ExistByConditionAsync(condition);
        }

        public ICollection<User> GetAll(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            return _userDecorator.GetAll(filter, orderedQuery, includedProperties, isTracked, isPaging, pageSize, page);
        }

        public async Task<ICollection<User>> GetAllAsync(Expression<Func<User, bool>>? filter = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderedQuery = null, string includedProperties = "", bool isTracked = true, bool isPaging = false, int pageSize = 10, int page = 1)
        {
            return await _userDecorator.GetAllAsync(filter, orderedQuery, includedProperties, isTracked, isPaging, pageSize, page);
        }

        public User? GetById(int id)
        {
            string key = $"user{id}";
            var cachedUser = _distributedCache.GetString(key);

            User? user;
            if (cachedUser == null)
            {
                user = _userDecorator.GetById(id);
                if (user == null)
                {
                    return user;
                }
                _distributedCache.SetString(key, JsonConvert.SerializeObject(user));
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
                user = await _userDecorator.GetByIdAsync(id);
                if (user != null)
                {
                    await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings));
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
                user = await _userDecorator.GetByIdAsync(id, includedProperties);
                if (user == null)
                {
                    return user;
                }
                await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings));
            }

            user = JsonConvert.DeserializeObject<User>(cachedUser, serializerSettings);
            return user;
        }

        public User? GetFirstOnCondition(Func<User, bool> condition)
        {
            return _userDecorator.GetFirstOnCondition(condition);
        }

        public async Task<User?> GetFirstOnConditionAsync(Expression<Func<User, bool>> condition)
        {
            return await _userDecorator.GetFirstOnConditionAsync(condition);
        }

        public User Update(User entity)
        {
            return _userDecorator.Update(entity);
        }
    }
}
