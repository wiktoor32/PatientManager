using Microsoft.EntityFrameworkCore;
using PatientManager.Api.Data;
using PatientManager.Api.Enums;
using PatientManager.Api.Models;

namespace PatientManager.Api.Services.Appointments;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _context;

    public AppointmentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Appointment>> GetAppointmentsAsync()
    {
        return await _context.Appointments
            .AsNoTracking()
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    // Basic operations
    public async Task<Appointment?> GetAppointmentAsync(int id)
    {
        return await _context.Appointments
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AppointmentResult> CreateAppointmentAsync(Appointment newAppointment)
    {
        if (newAppointment.ScheduledAt <= DateTimeOffset.UtcNow)
        {
            return new AppointmentResult(null, AppointmentError.InvalidDate);
        }

        if (newAppointment.DurationMinutes <= 0)
        {
            return new AppointmentResult(null, AppointmentError.InvalidDuration);
        }

        Patient? patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == newAppointment.PatientId);

        if (patient is null)
        {
            return new AppointmentResult(null, AppointmentError.PatientNotFound);
        }

        if (!patient.IsActive)
        {
            return new AppointmentResult(null, AppointmentError.PatientInactive);
        }

        Doctor? doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == newAppointment.DoctorId);

        if (doctor is null)
        {
            return new AppointmentResult(null, AppointmentError.DoctorNotFound);
        }

        if (!doctor.IsActive)
        {
            return new AppointmentResult(null, AppointmentError.DoctorInactive);
        }

        bool timeConflict = await HasTimeConflictAsync(
            newAppointment.DoctorId,
            newAppointment.ScheduledAt,
            newAppointment.DurationMinutes);

        if (timeConflict)
        {
            return new AppointmentResult(null, AppointmentError.TimeConflict);
        }

        newAppointment.Status = AppointmentStatus.Scheduled;

        _context.Appointments.Add(newAppointment);

        await _context.SaveChangesAsync();

        return new AppointmentResult(newAppointment, null);
    }

    public async Task<AppointmentResult> UpdateAppointmentAsync(int id, Appointment appointmentData)
    {
        Appointment? existingAppointment = await GetAppointmentAsync(id);

        if (existingAppointment is null)
        {
            return new AppointmentResult(null, AppointmentError.AppointmentNotFound);
        }

        if (existingAppointment.Status != AppointmentStatus.Scheduled ||
            existingAppointment.ScheduledAt <= DateTimeOffset.UtcNow)
        {
            return new AppointmentResult(null, AppointmentError.EditNotAllowed);
        }

        if (appointmentData.ScheduledAt <= DateTimeOffset.UtcNow)
        {
            return new AppointmentResult(null, AppointmentError.InvalidDate);
        }

        if (appointmentData.DurationMinutes <= 0)
        {
            return new AppointmentResult(null, AppointmentError.InvalidDuration);
        }

        Patient? patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Id == existingAppointment.PatientId);

        if (patient is null)
        {
            return new AppointmentResult(null, AppointmentError.PatientNotFound);
        }

        if (!patient.IsActive)
        {
            return new AppointmentResult(null, AppointmentError.PatientInactive);
        }

        Doctor? doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.Id == appointmentData.DoctorId);

        if (doctor is null)
        {
            return new AppointmentResult(null, AppointmentError.DoctorNotFound);
        }

        if (!doctor.IsActive)
        {
            return new AppointmentResult(null, AppointmentError.DoctorInactive);
        }

        bool timeConflict = await HasTimeConflictAsync(
            appointmentData.DoctorId,
            appointmentData.ScheduledAt,
            appointmentData.DurationMinutes,
            existingAppointment.Id);

        if (timeConflict)
        {
            return new AppointmentResult(null, AppointmentError.TimeConflict);
        }

        existingAppointment.DoctorId = appointmentData.DoctorId;
        existingAppointment.ScheduledAt = appointmentData.ScheduledAt;
        existingAppointment.DurationMinutes = appointmentData.DurationMinutes;
        existingAppointment.Description = appointmentData.Description;

        await _context.SaveChangesAsync();

        return new AppointmentResult(existingAppointment, null);
    }

    // Additional operations
    public async Task<List<Appointment>?> GetPatientAppointmentsAsync(int patientId)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
        {
            return null;
        }

        return await _context.Appointments
            .AsNoTracking()
            .Where(a => a.PatientId == patientId)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    public async Task<List<Appointment>?> GetDoctorAppointmentsAsync(int doctorId)
    {
        bool doctorExists = await _context.Doctors
            .AnyAsync(d => d.Id == doctorId);

        if (!doctorExists)
        {
            return null;
        }

        return await _context.Appointments
            .AsNoTracking()
            .Where(a => a.DoctorId == doctorId)
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync();
    }

    public async Task<Appointment?> GetAppointmentDetailsAsync(int id)
    {
        return await _context.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<AppointmentResult> UpdateAppointmentStatusAsync(int id, AppointmentStatus status)
    {
        Appointment? existingAppointment = await GetAppointmentAsync(id);

        if (existingAppointment is null)
        {
            return new AppointmentResult(null, AppointmentError.AppointmentNotFound);
        }

        if (existingAppointment.Status != AppointmentStatus.Scheduled)
        {
            return new AppointmentResult(null, AppointmentError.StatusChangeNotAllowed);
        }

        if (status is not (AppointmentStatus.Completed or AppointmentStatus.Cancelled or AppointmentStatus.NoShow))
        {
            return new AppointmentResult(null, AppointmentError.StatusChangeNotAllowed);
        }

        bool hasStarted = existingAppointment.ScheduledAt <= DateTimeOffset.UtcNow;

        if (status == AppointmentStatus.Cancelled && hasStarted)
        {
            return new AppointmentResult(null, AppointmentError.StatusChangeNotAllowed);
        }

        if (status != AppointmentStatus.Cancelled && !hasStarted)
        {
            return new AppointmentResult(null, AppointmentError.StatusChangeNotAllowed);
        }

        existingAppointment.Status = status;

        await _context.SaveChangesAsync();

        return new AppointmentResult(existingAppointment, null);
    }

    // Private helper method
    private async Task<bool> HasTimeConflictAsync(
        int doctorId,
        DateTimeOffset scheduledAt,
        int durationMinutes,
        int? excludedAppointmentId = null)
    {
        DateTimeOffset endAt = scheduledAt.AddMinutes(durationMinutes);

        return await _context.Appointments
            .AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.Status != AppointmentStatus.Cancelled &&
                (excludedAppointmentId == null || a.Id != excludedAppointmentId) &&
                a.ScheduledAt < endAt &&
                a.ScheduledAt.AddMinutes(a.DurationMinutes) > scheduledAt);
    }
}