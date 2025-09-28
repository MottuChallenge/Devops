using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(a => a.Street)
                   .HasColumnName("street")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(a => a.Number)
                   .HasColumnName("number")
                   .IsRequired();

            builder.Property(a => a.Neighborhood)
                   .HasColumnName("neighborhood")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.City)
                   .HasColumnName("city")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.State)
                   .HasColumnName("state")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(a => a.ZipCode)
                   .HasColumnName("zip_code")
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(a => a.Country)
                   .HasColumnName("country")
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
