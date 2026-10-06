using PatientManager.Api.Models;

namespace PatientManager.Api.Services;

public interface IPatientService
{
    //Basic operations
    List<Patient> GetPatients();
    Patient? GetPatient(int id);
    Patient CreatePatient(Patient newPatient);
    Patient? UpdatePatient(int id, Patient patientData);
    bool DeletePatient(int id);
    
    //Additional operations
    List<Patient> SearchByLastName(string lastName);
    List<Patient> FilterByActiveStatus(bool active);
    Patient? UpdateActiveStatus(int id, bool active);
}