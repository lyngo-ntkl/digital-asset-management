using DigitalAssetManagement.Application.Repositories;
using DigitalAssetManagement.Application.Services;
using DigitalAssetManagement.Infrastructure.Common;
using DigitalAssetManagement.Infrastructure.Common.Mappers;
using DigitalAssetManagement.Infrastructure.DatabaseContext;
using DigitalAssetManagement.Infrastructure.Repositories;
using DigitalAssetManagement.Infrastructure.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // dbcontext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("defaultConnection"));
                options.UseLazyLoadingProxies();
            });

            // mapper
            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(DriveMappingProfile));
            services.AddAutoMapper(typeof(FolderMappingProfile));
            services.AddAutoMapper(typeof(FileMappingProfile));
            services.AddAutoMapper(typeof(PermissionMappingProfile));

            // hangfire
            services.AddHangfire(options =>
            {
                options.UsePostgreSqlStorage(opts => opts.UseNpgsqlConnection(configuration.GetConnectionString("defaultConnection")));
            });
            services.AddHangfireServer();

            // repositories
            services.AddScoped<UnitOfWork, UnitOfWorkImplementation>();
            services.AddScoped<UserRepository, UserRepositoryImplementation>();
            services.AddScoped<DriveRepository, DriveRepositoryImplementation>();
            services.AddScoped<FolderRepository, FolderRepositoryImplementation>();
            services.AddScoped<FileRepository, FileRepositoryImplementation>();
            services.AddScoped<PermissionRepository, PermissionRepositoryImplementation>();

            // services
            services.AddScoped<UserService, UserServiceImplementation>();
            services.AddScoped<DriveService, DriveServiceImplementation>();
            services.AddScoped<FolderService, FolderServiceImplementation>();
            services.AddScoped<PermissionService, PermissionServiceImplementation>();

            // helper
            services.AddSingleton<HashingHelper, HashingHelperImplementation>();
            services.AddSingleton<JwtHelper, JwtHelperImplementation>();

            return services;
        }
    }
}
