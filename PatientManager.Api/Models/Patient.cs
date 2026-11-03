using PatientManager.Api.Enums;

namespace PatientManager.Api.Models;

public class Patient
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required DateOnly DateOfBirth { get; set; }

    public required string Pesel { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public EmergencyContact? EmergencyContact { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
        = new List<Appointment>();
}