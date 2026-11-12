namespace PatientManager.Api.Dtos.EmergencyContacts;

public class EmergencyContactDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Relationship { get; set; }
    public required string PhoneNumber { get; set; }
}