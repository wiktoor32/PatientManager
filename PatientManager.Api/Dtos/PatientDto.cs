using PatientManager.Api.Enums;

namespace PatientManager.Api.Dtos;

public class PatientDto
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }

    public required string Pesel { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }

    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}