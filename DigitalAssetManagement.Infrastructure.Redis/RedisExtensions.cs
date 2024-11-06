using DigitalAssetManagement.Infrastructure.Redis;
using DigitalAssetManagement.Infrastructure.Redis.Repositories;
using DigitalAssetManagement.UseCases;
using DigitalAssetManagement.UseCases.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Cache
{
    public static class RedisExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("redis");
            });

            services.AddScoped<ICache, CacheImplementation>();
            services.AddScoped<UserRepository, CachedUserRepositoryDecorator>();
        }
    }
}
