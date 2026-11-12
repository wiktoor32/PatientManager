using PatientManager.Api.Dtos.Doctors;
using PatientManager.Api.Models;

namespace PatientManager.Api.Mappings;

public static class DoctorMapper
{
    public static DoctorDto ToDoctorDto(Doctor doctor)
    {
        return new DoctorDto
        {
            Id = doctor.Id,
            FirstName = doctor.FirstName,
            LastName = doctor.LastName,
            Specialization = doctor.Specialization,
            IsActive = doctor.IsActive
        };
    }

    public static Doctor ToDoctor(CreateDoctorDto dto)
    {
        return new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Specialization = dto.Specialization
        };
    }

    public static Doctor ToDoctor(UpdateDoctorDto dto)
    {
        return new Doctor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Specialization = dto.Specialization
        };
    }

    public static DoctorDetailsDto ToDoctorDetailsDto(Doctor doctor)
    {
        return new DoctorDetailsDto
        {
            Doctor = ToDoctorDto(doctor),

            Appointments = doctor.Appointments
                .OrderByDescending(a => a.ScheduledAt)
                .Select(AppointmentMapper.ToDoctorAppointmentDto)
                .ToList()
        };
    }
}