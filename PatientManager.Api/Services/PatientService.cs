using PatientManager.Api.Models;

namespace PatientManager.Api.Services;

public class PatientService : IPatientService
{
    private static readonly List<Patient> s_patients =
    [
        new Patient
        {
            Id = 1,
            FirstName = "Anna",
            LastName = "Kowalska",
            DateOfBirth = new DateOnly(1998, 4, 12),
            Email = "anna.kowalska@example.com",
            IsActive = true
        },
        
        new Patient
        {
            Id = 2,
            FirstName = "Piotr",
            LastName = "Nowak",
            DateOfBirth = new DateOnly(1985, 11, 3),
            Email = "piotr.nowak@example.com",
            IsActive = true
        },
        
        new Patient
        {
            Id = 3,
            FirstName = "Marta",
            LastName = "Kowalczyk",
            DateOfBirth = new DateOnly(2001, 7, 25),
            Email = "marta.kowalczyk@example.com",
            IsActive = false
        }
    ];
    
    public List<Patient> GetPatients()
    {
        return s_patients;
    }

    public Patient? GetPatient(int id)
    {
        return s_patients
            .FirstOrDefault(p => p.Id == id);
    }

    public Patient CreatePatient(Patient newPatient)
    {
        newPatient.Id = s_patients.Count == 0 ? 1 : s_patients.Max(p => p.Id) + 1;
        newPatient.IsActive = true;
        
        s_patients.Add(newPatient);

        return newPatient;
    }

    public Patient? UpdatePatient(int id, Patient patientData)
    {
        Patient? existingPatient = GetPatient(id);

        if (existingPatient is null)
        {
            return null;
        }

        existingPatient.FirstName = patientData.FirstName;
        existingPatient.LastName = patientData.LastName;
        existingPatient.DateOfBirth = patientData.DateOfBirth;
        existingPatient.Email = patientData.Email;
        existingPatient.IsActive = patientData.IsActive;

        return existingPatient;
    }

    public bool DeletePatient(int id)
    {
        Patient? patientToDelete = GetPatient(id);

        if (patientToDelete is null)
        {
            return false;
        }
        
        return s_patients.Remove(patientToDelete);
    }

    public List<Patient> SearchByLastName(string lastName)
    {
        return s_patients
            .Where(p => p.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Patient> FilterByActiveStatus(bool active)
    {
        return s_patients
            .Where(p => p.IsActive == active)
            .ToList();
    }

    public Patient? UpdateActiveStatus(int id, bool active)
    {
        Patient? existingPatient = GetPatient(id);

        if (existingPatient is null)
        {
            return null;
        }

        existingPatient.IsActive = active;
        return existingPatient;
    }
}