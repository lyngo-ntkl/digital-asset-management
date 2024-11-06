using DigitalAssetManagement.Infrastructure.PostgreSQL.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public static class MigrationHelper
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbcontext.Database.Migrate();
        }
    }
}
