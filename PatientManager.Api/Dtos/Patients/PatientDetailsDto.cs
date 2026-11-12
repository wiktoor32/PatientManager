using PatientManager.Api.Dtos.EmergencyContacts;

namespace PatientManager.Api.Dtos.Patients;

public class PatientDetailsDto
{
    public required PatientDto Patient { get; set; }
    public EmergencyContactDto? EmergencyContact { get; set; }
    public List<PatientAppointmentDto> Appointments { get; set; } = [];
}