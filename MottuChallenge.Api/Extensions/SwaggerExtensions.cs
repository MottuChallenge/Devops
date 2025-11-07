using Microsoft.OpenApi.Models;
using MottuChallenge.Application.Configurations;

namespace MottuChallenge.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, SwaggerSettings settings)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = settings.Contact.Name,
                    Email = settings.Contact.Email,
                    Url = settings.Contact.Url
                }
            });
            
            swagger.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = settings.Contact.Name,
                    Email = settings.Contact.Email,
                    Url = settings.Contact.Url
                }
            });
            
            swagger.EnableAnnotations();
            
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Digite 'Bearer' [espaço] e o token JWT.\n\nExemplo: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9**"
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }
}