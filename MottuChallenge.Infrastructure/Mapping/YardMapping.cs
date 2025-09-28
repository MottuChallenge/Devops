using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;
using MottuChallenge.Domain.ValueObjects;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class YardMapping : IEntityTypeConfiguration<Yard>
    {
        public void Configure(EntityTypeBuilder<Yard> builder)
        {
            builder.ToTable("yards");

            builder.HasKey(y => y.Id);

            builder.Property(y => y.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(y => y.Name)
                   .HasColumnName("name")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(y => y.AddressId)
                   .HasColumnName("address_id")
                   .IsRequired();

            builder.HasOne(y => y.Address)
                   .WithOne()
                   .HasForeignKey<Yard>(y => y.AddressId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(y => y.Sectors)
                   .WithOne(s => s.Yard)
                   .HasForeignKey(s => s.YardId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsMany<PolygonPoint>(y => y.Points, pb =>
            {
                pb.ToTable("yard_points");

                pb.WithOwner().HasForeignKey("YardId");

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
        }
    }
}
