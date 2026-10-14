using PatientManager.Api.Models;

namespace PatientManager.Api.Services;

public interface IPatientService
{
    //Basic operations
    Task<List<Patient>> GetPatientsAsync();
    Task<Patient?> GetPatientAsync(int id);
    Task<Patient> CreatePatientAsync(Patient newPatient);
    Task<Patient?> UpdatePatientAsync(int id, Patient patientData);
    Task<bool> DeletePatientAsync(int id);
    
    //Additional operations
    Task<List<Patient>> SearchByLastNameAsync(string lastName);
    Task<List<Patient>> FilterByActiveStatusAsync(bool active);
    Task<Patient?> UpdateActiveStatusAsync(int id, bool active);
}