using PatientManager.Api.Enums;
using PatientManager.Api.Models;

namespace PatientManager.Api.Services.Appointments;

public interface IAppointmentService
{
    // Basic operations
    Task<List<Appointment>> GetAppointmentsAsync();
    Task<Appointment?> GetAppointmentAsync(int id);
    Task<AppointmentResult> CreateAppointmentAsync(Appointment newAppointment);
    Task<AppointmentResult> UpdateAppointmentAsync(int id, Appointment appointmentData);

    // Additional operations
    Task<List<Appointment>?> GetPatientAppointmentsAsync(int patientId);
    Task<List<Appointment>?> GetDoctorAppointmentsAsync(int doctorId);
    Task<Appointment?> GetAppointmentDetailsAsync(int id);
    Task<AppointmentResult> UpdateAppointmentStatusAsync(int id, AppointmentStatus status);
}