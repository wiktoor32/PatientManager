using PatientManager.Api.Dtos.Doctors;
using PatientManager.Api.Enums;

namespace PatientManager.Api.Dtos.Patients;

public class PatientAppointmentDto
{
    public int Id { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Description { get; set; }

    public required DoctorDto Doctor { get; set; }
}