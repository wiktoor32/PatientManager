namespace PatientManager.Api.Services.Appointments;

public enum AppointmentError
{
    AppointmentNotFound,
    PatientNotFound,
    DoctorNotFound,
    PatientInactive,
    DoctorInactive,
    InvalidDate,
    InvalidDuration,
    TimeConflict,
    EditNotAllowed,
    StatusChangeNotAllowed
}