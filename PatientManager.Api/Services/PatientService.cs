using Microsoft.EntityFrameworkCore;
using PatientManager.Api.Data;
using PatientManager.Api.Models;

namespace PatientManager.Api.Services;

public class PatientService : IPatientService
{
    private readonly AppDbContext _context;

    public PatientService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Patient>> GetPatientsAsync()
    {
        return await _context.Patients
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public async Task<Patient?> GetPatientAsync(int id)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient> CreatePatientAsync(Patient newPatient)
    {
        newPatient.IsActive = true;
        
        _context.Patients.Add(newPatient);

        await _context.SaveChangesAsync();

        return newPatient;
    }

    public async Task<Patient?> UpdatePatientAsync(int id, Patient patientData)
    {
        Patient? existingPatient = await GetPatientAsync(id);

        if (existingPatient is null)
        {
            return null;
        }

        existingPatient.FirstName = patientData.FirstName;
        existingPatient.LastName = patientData.LastName;
        existingPatient.DateOfBirth = patientData.DateOfBirth;
        existingPatient.Email = patientData.Email;

        await _context.SaveChangesAsync();

        return existingPatient;
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        Patient? patientToDelete = await GetPatientAsync(id);

        if (patientToDelete is null)
        {
            return false;
        }

        _context.Patients.Remove(patientToDelete);
        
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Patient>> SearchByLastNameAsync(string lastName)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.LastName.Contains(lastName))
            .ToListAsync();
    }

    public async Task<List<Patient>> FilterByActiveStatusAsync(bool active)
    {
        return await _context.Patients
            .AsNoTracking()
            .Where(p => p.IsActive == active)
            .ToListAsync();
    }

    public async Task<Patient?> UpdateActiveStatusAsync(int id, bool active)
    {
        Patient? existingPatient = await GetPatientAsync(id);

        if (existingPatient is null)
        {
            return null;
        }

        existingPatient.IsActive = active;

        await _context.SaveChangesAsync();

        return existingPatient;
    }
}