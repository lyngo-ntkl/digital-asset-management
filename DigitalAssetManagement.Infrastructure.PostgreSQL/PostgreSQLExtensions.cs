using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.Common.Mappers;
using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.Infrastructure.Repositories;
using DigitalAssetManagement.UseCases.Common;
using DigitalAssetManagement.UseCases.Repositories;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure
{
    public static class PostgreSQLExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("defaultConnection"));
                options.UseLazyLoadingProxies();
            });

            services.AddScoped<MetadataPermissionUnitOfWork, >();
            services.AddScoped<MetadataRepository, MetadataRepositoryImplementation>();
            services.AddScoped<PermissionRepository, PermissionRepositoryImplementation>();
            services.AddScoped<UserRepositoryImplementation>();




            

            // mapper
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(PermissionMappingProfile));
            services.AddAutoMapper(typeof(MetadataMappingProfile));

            // hangfire
            services.AddHangfire(options =>
            {
                options.UsePostgreSqlStorage(opts => opts.UseNpgsqlConnection(configuration.GetConnectionString("defaultConnection")));
            });
            services.AddHangfireServer();

            // rabbitmq
            services.AddHostedService<RabbitMQBackgroundService>();

            // helper
            services.AddSingleton<HashingHelper, HashingHelperImplementation>();
            services.AddSingleton<JwtHelper, JwtHelperImplementation>();
            services.AddSingleton<SystemFileHelper, SystemFileHelperImplementation>();
            services.AddScoped<SystemFolderHelper, SystemFolderHelperImplementation>();

            return services;
        }
    }
}
