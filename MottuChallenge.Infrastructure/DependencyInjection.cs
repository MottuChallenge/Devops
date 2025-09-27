using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MottuChallenge.Application.Interfaces;
using MottuChallenge.Application.Repositories;
using MottuChallenge.Infrastructure.Persistence;
using MottuChallenge.Infrastructure.Repositories;
using MottuChallenge.Infrastructure.Services;

namespace MottuChallenge.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
                                   ?? configuration.GetConnectionString("MySqlConnection");
            Console.WriteLine($"DB_CONNECTION = {connectionString}, ");
            services.AddDbContext<MottuChallengeContext>(options =>
                options.UseMySQL(connectionString, mySqlOptions =>
                    mySqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 15,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                )   
            );

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IYardRepository, YardRepository>();
            services.AddScoped<ISectorRepository, SectorRepository>();
            services.AddScoped<ISectorTypeRepository, SectorTypeRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAddressProvider, FindAddressByApiViaCep>();
            return services;
        }

    }
}
