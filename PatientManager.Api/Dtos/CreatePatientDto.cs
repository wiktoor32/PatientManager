namespace PatientManager.Api.Dtos;

public class CreatePatientDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
}