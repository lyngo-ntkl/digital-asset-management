using DigitalAssetManagement.UseCases.Common;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure.Hangfire
{
    public static class HangfireExtensions
    {
        public static void AddHangfireConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(options =>
            {
                options.UsePostgreSqlStorage(opts => opts.UseNpgsqlConnection(configuration.GetConnectionString("defaultConnection")));
            });
            services.AddHangfireServer();

            services.AddScoped<IScheduler, SchedulerImplementation>();
        }
    }
}
