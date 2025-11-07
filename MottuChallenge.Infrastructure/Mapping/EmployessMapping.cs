using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MottuChallenge.Domain.Entities;

namespace MottuChallenge.Infrastructure.Mapping;

public class EmployessMapping : IEntityTypeConfiguration<Employee>
{
    
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasColumnName("id")
               .IsRequired();

        builder.Property(e => e.Name)
               .HasColumnName("name")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(e => e.Email)
               .HasColumnName("email")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(e => e.YardId)
               .HasColumnName("yard_id")
               .IsRequired();
        
        builder.HasOne(e => e.Yard)
               .WithMany()
               .HasForeignKey(e => e.YardId)
               .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(e => e.PasswordHash)
               .HasColumnName("password_hash")
               .IsRequired();
        
        builder.Property(e => e.PasswordSalt)
               .HasColumnName("password_salt")
               .IsRequired();
              

    }
    
}