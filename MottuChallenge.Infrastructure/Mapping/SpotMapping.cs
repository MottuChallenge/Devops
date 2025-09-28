using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class SpotMapping : IEntityTypeConfiguration<Spot>
    {
        public void Configure(EntityTypeBuilder<Spot> builder)
        {
            builder.ToTable("spots");

            builder.HasKey(s => s.SpotId);

            builder.Property(s => s.SpotId)
                   .HasColumnName("spot_id")
                   .IsRequired();

            builder.Property(s => s.SectorId)
                   .HasColumnName("sector_id")
                   .IsRequired();

            builder.Property(s => s.X)
                   .HasColumnName("x")
                   .IsRequired();

            builder.Property(s => s.Y)
                   .HasColumnName("y")
                   .IsRequired();

            builder.Property(s => s.Status)
                   .HasColumnName("status")
                   .HasConversion<string>() 
                   .IsRequired();

            builder.Property(s => s.MotorcycleId)
                   .HasColumnName("motorcycle_id")
                   .IsRequired(false);

            builder.HasOne(s => s.Sector)
                   .WithMany(sec => sec.Spots)
                   .HasForeignKey(s => s.SectorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Motorcycle)
                   .WithOne()
                   .HasForeignKey<Spot>(s => s.MotorcycleId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
