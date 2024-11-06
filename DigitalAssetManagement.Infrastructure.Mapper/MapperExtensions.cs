using DigitalAssetManagement.Infrastructure.Mapper.MapperProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssetManagement.Infrastructure.Mapper
{
    public static class MapperExtensions
    {
        public static void AddMappers(this IServiceCollection services)
        {
            services.AddScoped<UseCases.Common.IMapper, MapperImplementation>();

            services.AddAutoMapper(typeof(UserMappingProfile));
            services.AddAutoMapper(typeof(PermissionMappingProfile));
            services.AddAutoMapper(typeof(MetadataMappingProfile));
        }
    }
}
