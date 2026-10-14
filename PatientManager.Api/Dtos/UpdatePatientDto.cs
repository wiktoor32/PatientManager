using System.ComponentModel.DataAnnotations;
using PatientManager.Api.Validation;

namespace PatientManager.Api.Dtos;

public class UpdatePatientDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [ValidBirthDate]
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;
}