using PatientManager.Api.Dtos.Appointments;
using PatientManager.Api.Dtos.Doctors;
using PatientManager.Api.Dtos.Patients;
using PatientManager.Api.Enums;
using PatientManager.Api.Models;

namespace PatientManager.Api.Mappings;

public static class AppointmentMapper
{
    public static AppointmentDto ToAppointmentDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            DoctorId = appointment.DoctorId,
            ScheduledAt = appointment.ScheduledAt,
            DurationMinutes = appointment.DurationMinutes,
            Status = appointment.Status,
            Description = appointment.Description
        };
    }

    public static Appointment ToAppointment(CreateAppointmentDto dto)
    {
        return new Appointment
        {
            PatientId = dto.PatientId,
            DoctorId = dto.DoctorId,
            ScheduledAt = dto.ScheduledAt,
            DurationMinutes = dto.DurationMinutes,
            Status = AppointmentStatus.Scheduled,
            Description = dto.Description
        };
    }

    public static Appointment ToAppointment(UpdateAppointmentDto dto)
    {
        return new Appointment
        {
            DoctorId = dto.DoctorId,
            ScheduledAt = dto.ScheduledAt,
            DurationMinutes = dto.DurationMinutes,
            Status = AppointmentStatus.Scheduled,
            Description = dto.Description
        };
    }

    public static AppointmentDetailsDto ToAppointmentDetailsDto(Appointment appointment)
    {
        return new AppointmentDetailsDto
        {
            Id = appointment.Id,
            ScheduledAt = appointment.ScheduledAt,
            DurationMinutes = appointment.DurationMinutes,
            Status = appointment.Status,
            Description = appointment.Description,
            Patient = PatientMapper.ToPatientSummaryDto(appointment.Patient),
            Doctor = DoctorMapper.ToDoctorDto(appointment.Doctor)
        };
    }

    public static PatientAppointmentDto ToPatientAppointmentDto(Appointment appointment)
    {
        return new PatientAppointmentDto
        {
            Id = appointment.Id,
            ScheduledAt = appointment.ScheduledAt,
            DurationMinutes = appointment.DurationMinutes,
            Status = appointment.Status,
            Description = appointment.Description,
            Doctor = DoctorMapper.ToDoctorDto(appointment.Doctor)
        };
    }

    public static DoctorAppointmentDto ToDoctorAppointmentDto(Appointment appointment)
    {
        return new DoctorAppointmentDto
        {
            Id = appointment.Id,
            ScheduledAt = appointment.ScheduledAt,
            DurationMinutes = appointment.DurationMinutes,
            Status = appointment.Status,
            Description = appointment.Description,
            Patient = PatientMapper.ToPatientSummaryDto(appointment.Patient)
        };
    }
}