using PatientManager.Api.Models;

namespace PatientManager.Api.Services.Appointments;

public record AppointmentResult(
    Appointment? Appointment,
    AppointmentError? Error);