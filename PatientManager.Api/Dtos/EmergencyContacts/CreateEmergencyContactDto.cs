using System.ComponentModel.DataAnnotations;
using PatientManager.Api.Validation;

namespace PatientManager.Api.Dtos.EmergencyContacts;

public class CreateEmergencyContactDto
{
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Relationship { get; set; }

    [Required]
    [ValidPhoneNumber]
    public required string PhoneNumber { get; set; }
}