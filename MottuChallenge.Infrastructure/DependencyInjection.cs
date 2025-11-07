using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MottuChallenge.Application.Configurations;
using MottuChallenge.Application.Interfaces;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Infrastructure.Persistence;
using MottuChallenge.Infrastructure.Repositories;
using MottuChallenge.Infrastructure.Security;
using MottuChallenge.Infrastructure.Services;

namespace MottuChallenge.Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection AddDbContext(this IServiceCollection services, ConnectionSettings connectionSettings)
        {
            var connectionString = connectionSettings.MysqlConnection;
            services.AddDbContext<MottuChallengeContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
                { 
                    mysqlOptions.EnableRetryOnFailure();
                });
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IYardRepository, YardRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<ISectorTypeRepository, SectorTypeRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAddressProvider, FindAddressByApiViaCep>();
            return services;
        }

        private static IServiceCollection AddSecurity(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    var secret = jwtSettings.SecretKey;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
            services.AddScoped<JwtTokenService>();
            return services;
        }
        
        
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Settings settings)
        {
            services.AddDbContext(settings.ConnectionStrings);
            services.AddSecurity(settings.Jwt);
            services.AddRepositories();
            services.AddServices();
            return services;
        }

    }
}
