using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping
{
    internal class SectorTypeMapping : IEntityTypeConfiguration<SectorType>
    {
        public void Configure(EntityTypeBuilder<SectorType> builder)
        {
            builder.ToTable("sector_types");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.Id)
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(st => st.Name)
                   .HasColumnName("name")
                   .HasMaxLength(100)
                   .IsRequired();
        }
    }
}
