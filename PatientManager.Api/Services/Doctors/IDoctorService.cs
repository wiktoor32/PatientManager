using PatientManager.Api.Models;

namespace PatientManager.Api.Services.Doctors;

public interface IDoctorService
{
    //Basic operations
    Task<List<Doctor>> GetDoctorsAsync();
    Task<Doctor?> GetDoctorAsync(int id);
    Task<Doctor> CreateDoctorAsync(Doctor newDoctor);
    Task<Doctor?> UpdateDoctorAsync(int id, Doctor doctorData);

    // Additional operations
    Task<List<Doctor>> SearchByLastNameAsync(string lastName);
    Task<List<Doctor>> SearchBySpecializationAsync(string specialization);
    Task<List<Doctor>> FilterByActiveStatusAsync(bool active);
    Task<Doctor?> GetDoctorDetailsAsync(int id);
    Task<Doctor?> UpdateActiveStatusAsync(int id, bool active);
}