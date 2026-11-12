using PatientManager.Api.Enums;

namespace PatientManager.Api.Dtos.Appointments;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    public DateTimeOffset ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }

    public string? Description { get; set; }
}