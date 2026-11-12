using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientManager.Api.Models;

namespace PatientManager.Api.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.FirstName)
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .HasMaxLength(100);

        builder.Property(p => p.Pesel)
            .HasMaxLength(11);

        builder.Property(p => p.Email)
            .HasMaxLength(254);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(40);

        builder.HasIndex(p => p.Pesel)
            .IsUnique();
    }
}