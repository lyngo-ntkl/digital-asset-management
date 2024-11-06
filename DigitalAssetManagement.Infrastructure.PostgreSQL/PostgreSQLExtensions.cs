using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using DigitalAssetManagement.Infrastructure.PostgreSQL.Repositories;
using DigitalAssetManagement.UseCases.Repositories;
using DigitalAssetManagement.UseCases.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure.PostgreSQL
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

            services.AddScoped<IMetadataRepository, MetadataRepositoryImplementation>();
            services.AddScoped<IPermissionRepository, PermissionRepositoryImplementation>();
            services.AddScoped<UserRepositoryImplementation>();

            return services;
        }
    }
}
