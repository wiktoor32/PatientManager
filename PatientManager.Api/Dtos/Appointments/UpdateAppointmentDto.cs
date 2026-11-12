using System.ComponentModel.DataAnnotations;

namespace PatientManager.Api.Dtos.Appointments;

public class UpdateAppointmentDto
{
    [Range(1, int.MaxValue)]
    public required int DoctorId { get; set; }

    public required DateTimeOffset ScheduledAt { get; set; }

    [Range(1, int.MaxValue)]
    public required int DurationMinutes { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}