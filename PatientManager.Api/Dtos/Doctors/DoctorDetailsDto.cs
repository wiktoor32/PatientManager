namespace PatientManager.Api.Dtos.Doctors;

public class DoctorDetailsDto
{
    public required DoctorDto Doctor { get; set; }
    public List<DoctorAppointmentDto> Appointments { get; set; } = [];
}