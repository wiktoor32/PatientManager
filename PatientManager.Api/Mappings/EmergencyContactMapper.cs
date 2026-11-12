using PatientManager.Api.Dtos.EmergencyContacts;
using PatientManager.Api.Models;

namespace PatientManager.Api.Mappings;

public static class EmergencyContactMapper
{
    public static EmergencyContactDto ToEmergencyContactDto(EmergencyContact contact)
    {
        return new EmergencyContactDto
        {
            Id = contact.Id,
            PatientId = contact.PatientId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            Relationship = contact.Relationship,
            PhoneNumber = contact.PhoneNumber
        };
    }

    public static EmergencyContact ToEmergencyContact(
        CreateEmergencyContactDto dto,
        int patientId,
        string normalizedPhone)
    {
        return new EmergencyContact
        {
            PatientId = patientId,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Relationship = dto.Relationship,
            PhoneNumber = normalizedPhone
        };
    }

    public static EmergencyContact ToEmergencyContact(UpdateEmergencyContactDto dto, string normalizedPhone)
    {
        return new EmergencyContact
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Relationship = dto.Relationship,
            PhoneNumber = normalizedPhone
        };
    }
}