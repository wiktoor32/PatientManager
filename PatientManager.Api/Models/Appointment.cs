using PatientManager.Api.Enums;

namespace PatientManager.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }

    public required DateTimeOffset ScheduledAt { get; set; }
    public required int DurationMinutes { get; set; }
    public required AppointmentStatus Status { get; set; }

    public string? Description { get; set; }

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}