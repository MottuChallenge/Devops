using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("Motorcycles");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.EngineType)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(m => m.Plate)
                   .IsRequired()
                   .HasMaxLength(8);

            builder.Property(m => m.LastRevisionDate)
                   .IsRequired();

            builder.HasOne(m => m.Spot)
                   .WithOne(s => s.Motorcycle)
                   .HasForeignKey<Motorcycle>(m => m.SpotId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
