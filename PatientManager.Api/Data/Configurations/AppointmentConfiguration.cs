using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientManager.Api.Models;

namespace PatientManager.Api.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(a => a.Description)
            .HasMaxLength(500);
    }
}