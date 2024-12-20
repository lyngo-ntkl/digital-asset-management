﻿using DigitalAssetManagement.API.Common;
using DigitalAssetManagement.API.Common.Authorizations;
using DigitalAssetManagement.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DigitalAssetManagement.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]!)),
                    ValidIssuer = configuration["jwt:issuer"]
                };
            });

            services.AddTransient<IAuthorizationHandler, CustomAuthorizationHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Contributor", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new CustomAuthorizationRequirement(Role.Contributor));
                });
                options.AddPolicy("Reader", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new CustomAuthorizationRequirement(Role.Reader));
                });
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new CustomAuthorizationRequirement(Role.Admin));
                });
            });

            // httpcontextaccessor
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
