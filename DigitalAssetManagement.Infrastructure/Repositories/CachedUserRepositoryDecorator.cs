﻿using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Domain.Entities;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

        public Task<User> AddAsync(User entity)
        {
            return _userRepository.AddAsync(entity);
        }

        public async Task<bool> ExistByEmailAsync(string email)
        {
            return await _userRepository.ExistByEmailAsync(email);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public void Delete(User entity)
        {
            _userRepository.Delete(entity);
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

        public async void Update(User user)
        {
            _userRepository.Update(user);

            string key = $"user{user.Id}";
            if (await _distributedCache.GetAsync(key) != null)
            {
                await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(user, serializerSettings), _options);
            }
        }
    }
}
