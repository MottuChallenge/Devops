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
            services.AddDbContext<MottuChallengeContext>(options =>
            {
                options.UseMySQL(configuration.GetConnectionString("MySqlConnection"));
            });

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
