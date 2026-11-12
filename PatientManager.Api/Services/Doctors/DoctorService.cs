using Microsoft.EntityFrameworkCore;
using PatientManager.Api.Data;
using PatientManager.Api.Models;

namespace PatientManager.Api.Services.Doctors;

public class DoctorService : IDoctorService
{
    private readonly AppDbContext _context;

    public DoctorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Doctor>> GetDoctorsAsync()
    {
        return await _context.Doctors
            .AsNoTracking()
            .OrderBy(d => d.Id)
            .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorAsync(int id)
    {
        return await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Doctor> CreateDoctorAsync(Doctor newDoctor)
    {
        newDoctor.IsActive = true;

        _context.Doctors.Add(newDoctor);

        await _context.SaveChangesAsync();

        return newDoctor;
    }

    public async Task<Doctor?> UpdateDoctorAsync(int id, Doctor doctorData)
    {
        Doctor? existingDoctor = await GetDoctorAsync(id);

        if (existingDoctor is null)
        {
            return null;
        }

        existingDoctor.FirstName = doctorData.FirstName;
        existingDoctor.LastName = doctorData.LastName;
        existingDoctor.Specialization = doctorData.Specialization;

        await _context.SaveChangesAsync();

        return existingDoctor;
    }

    public async Task<List<Doctor>> SearchByLastNameAsync(string lastName)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Where(d => d.LastName.Contains(lastName))
            .ToListAsync();
    }

    public async Task<List<Doctor>> SearchBySpecializationAsync(string specialization)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Where(d => d.Specialization.Contains(specialization))
            .ToListAsync();
    }

    public async Task<List<Doctor>> FilterByActiveStatusAsync(bool active)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Where(d => d.IsActive == active)
            .ToListAsync();
    }

    public async Task<Doctor?> GetDoctorDetailsAsync(int id)
    {
        return await _context.Doctors
            .AsNoTracking()
            .Include(d => d.Appointments)
            .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Doctor?> UpdateActiveStatusAsync(int id, bool active)
    {
        Doctor? existingDoctor = await GetDoctorAsync(id);

        if (existingDoctor is null)
        {
            return null;
        }

        existingDoctor.IsActive = active;

        await _context.SaveChangesAsync();

        return existingDoctor;
    }
}