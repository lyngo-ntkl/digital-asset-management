using DigitalAssetManagement.API.Common;

namespace DigitalAssetManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services)
        {
            // swagger
            services.AddSwaggerGen();

            // controllers
            services.AddControllers();

            // cors
            services.AddCors();

            // exception handler
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            // auth
            services.AddAuthentication();
            services.AddAuthorization();

            return services;
        }
    }
}
