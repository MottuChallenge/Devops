using Microsoft.EntityFrameworkCore;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Infrastructure.Mapping;

namespace MottuChallenge.Infrastructure.Persistence
{
    internal class MottuChallengeContext(DbContextOptions<MottuChallengeContext> options) : DbContext(options)
    {
        public DbSet<Yard> Yards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SectorType> SectorTypes { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new YardMapping());
            modelBuilder.ApplyConfiguration(new AddressMapping());
            modelBuilder.ApplyConfiguration(new SectorTypeMapping());
            modelBuilder.ApplyConfiguration(new SectorMapping());
            modelBuilder.ApplyConfiguration(new SpotMapping());
            modelBuilder.ApplyConfiguration(new MotorcycleMapping());
            modelBuilder.ApplyConfiguration(new LogMapping());
        }
    }
}
