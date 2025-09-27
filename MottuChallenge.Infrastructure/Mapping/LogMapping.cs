using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class LogMapping : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("logs");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(l => l.Message)
                   .HasColumnName("message")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(l => l.CreatedAt)
                   .HasColumnName("created_at")
                   .IsRequired();

            builder.Property(l => l.MotorcycleId)
                   .HasColumnName("motorcycle_id")
                   .IsRequired();

            builder.Property(l => l.PreviousSpotId)
                   .HasColumnName("previous_spot_id")
                   .IsRequired();

            builder.Property(l => l.DestinationSpotId)
                   .HasColumnName("destination_spot_id")
                   .IsRequired();

            builder.HasOne(l => l.Motorcycle)
                   .WithMany()
                   .HasForeignKey(l => l.MotorcycleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.PreviousSpot)
                   .WithMany()
                   .HasForeignKey(l => l.PreviousSpotId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.DestinationSpot)
                   .WithMany()
                   .HasForeignKey(l => l.DestinationSpotId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
