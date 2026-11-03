using System.ComponentModel.DataAnnotations;
using PatientManager.Api.Enums;
using PatientManager.Api.Validation;

namespace PatientManager.Api.Dtos;

public class CreatePatientDto
{
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [Range(1, 2, ErrorMessage = "Gender must be Male or Female.")]
    public required Gender Gender { get; set; }

    [ValidBirthDate]
    public required DateOnly DateOfBirth { get; set; }

    [Required]
    [ValidPesel]
    public required string Pesel { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public required string Email { get; set; }

    [Required]
    [ValidPhoneNumber]
    public required string PhoneNumber { get; set; }
}