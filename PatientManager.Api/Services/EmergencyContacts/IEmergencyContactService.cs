using PatientManager.Api.Models;

namespace PatientManager.Api.Services.EmergencyContacts;

public interface IEmergencyContactService
{
    Task<EmergencyContact?> GetEmergencyContactAsync(int patientId);
    Task<EmergencyContact?> CreateEmergencyContactAsync(EmergencyContact newContact);
    Task<EmergencyContact?> UpdateEmergencyContactAsync(int patientId, EmergencyContact contactData);
    Task<bool?> DeleteEmergencyContactAsync(int patientId);
}