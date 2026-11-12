namespace PatientManager.Api.Dtos.Patients;

public class PatientSummaryDto
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Pesel { get; set; }

    public bool IsActive { get; set; }
}