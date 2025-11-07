using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MottuChallenge.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MottuChallengeContext>();
            dbContext.Database.Migrate();
        }
    }
}
