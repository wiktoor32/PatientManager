using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PatientManager.Api.Models;

namespace PatientManager.Api.Data.Configurations;

public class EmergencyContactConfiguration : IEntityTypeConfiguration<EmergencyContact>
{
    public void Configure(EntityTypeBuilder<EmergencyContact> builder)
    {
        builder.HasOne(e => e.Patient)
            .WithOne(p => p.EmergencyContact)
            .OnDelete(DeleteBehavior.NoAction);
    }
}