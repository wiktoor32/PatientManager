using System.ComponentModel.DataAnnotations;

namespace PatientManager.Api.Dtos.Doctors;

public class UpdateDoctorDto
{
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Specialization { get; set; }
}