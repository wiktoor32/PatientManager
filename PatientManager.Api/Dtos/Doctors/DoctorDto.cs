namespace PatientManager.Api.Dtos.Doctors;

public class DoctorDto
{
    public int Id { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Specialization { get; set; }

    public bool IsActive { get; set; }
}