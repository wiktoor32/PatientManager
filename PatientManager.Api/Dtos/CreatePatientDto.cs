using System.ComponentModel.DataAnnotations;

namespace PatientManager.Api.Dtos;

public class CreatePatientDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    public DateOnly DateOfBirth { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;
}