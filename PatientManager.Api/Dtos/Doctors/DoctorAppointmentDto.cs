using PatientManager.Api.Dtos.Patients;
using PatientManager.Api.Enums;

namespace PatientManager.Api.Dtos.Doctors;

public class DoctorAppointmentDto
{
    public int Id { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Description { get; set; }

    public required PatientSummaryDto Patient { get; set; }
}