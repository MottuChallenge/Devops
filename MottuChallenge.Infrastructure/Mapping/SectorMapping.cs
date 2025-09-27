using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class SectorMapping : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("sectors");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(s => s.YardId)
                   .HasColumnName("yard_id")
                   .IsRequired();

            builder.Property(s => s.SectorTypeId)
                   .HasColumnName("sector_type_id")
                   .IsRequired();

            builder.HasOne(s => s.Yard)
                   .WithMany(y => y.Sectors)
                   .HasForeignKey(s => s.YardId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.SectorType)
                   .WithMany()
                   .HasForeignKey(s => s.SectorTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany<PolygonPoint>(s => s.Points, pb =>
            {
                pb.ToTable("sector_points");

                pb.WithOwner().HasForeignKey("SectorId");

                pb.Property<Guid>("Id")
                  .ValueGeneratedOnAdd();

                pb.Property(p => p.PointOrder)
                  .HasColumnName("point_order")
                  .IsRequired();

                pb.Property(p => p.X)
                  .HasColumnName("x")
                  .IsRequired();

                pb.Property(p => p.Y)
                  .HasColumnName("y")
                  .IsRequired();

                pb.HasKey("Id");
            });

            builder.HasMany(s => s.Spots)
                   .WithOne(sp => sp.Sector)
                   .HasForeignKey(sp => sp.SectorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
