using PatientManager.Api.Dtos;
using PatientManager.Api.Models;

namespace PatientManager.Api.Mappings;

public static class PatientMapper
{
    public static PatientDto ToPatientDto(Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Gender = patient.Gender,
            DateOfBirth = patient.DateOfBirth,
            Pesel = patient.Pesel,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            IsActive = patient.IsActive,
            CreatedAt = patient.CreatedAt
        };
    }

    public static PatientSummaryDto ToPatientSummaryDto(Patient patient)
    {
        return new PatientSummaryDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Pesel = patient.Pesel,
            IsActive = patient.IsActive
        };
    }

    public static Patient ToPatient(CreatePatientDto dto, string normalizedPhone)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Pesel = dto.Pesel,
            Email = dto.Email,
            PhoneNumber = normalizedPhone
        };
    }

    public static Patient ToPatient(UpdatePatientDto dto, string normalizedPhone)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Gender = dto.Gender,
            DateOfBirth = dto.DateOfBirth,
            Pesel = dto.Pesel,
            Email = dto.Email,
            PhoneNumber = normalizedPhone
        };
    }
}