using DigitalAssetManagement.API.Common;
using Microsoft.OpenApi.Models;

namespace DigitalAssetManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services)
        {
            // swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Digital Asset Management API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

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

            // httpcontextaccessor
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
