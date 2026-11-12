using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientManager.Api.Models;

namespace PatientManager.Api.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.Property(d => d.FirstName)
            .HasMaxLength(100);

        builder.Property(d => d.LastName)
            .HasMaxLength(100);

        builder.Property(d => d.Specialization)
            .HasMaxLength(100);
    }
}