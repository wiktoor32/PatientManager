using Microsoft.AspNetCore.Mvc;
using PatientManager.Api.Dtos.Appointments;
using PatientManager.Api.Enums;
using PatientManager.Api.Mappings;
using PatientManager.Api.Models;
using PatientManager.Api.Services.Appointments;

namespace PatientManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // Basic operations
    [HttpGet]
    public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsAsync()
    {
        List<Appointment> appointments = await _appointmentService.GetAppointmentsAsync();

        return appointments
            .Select(AppointmentMapper.ToAppointmentDto)
            .ToList();
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetAppointmentAsync))]
    public async Task<ActionResult<AppointmentDto>> GetAppointmentAsync(
        [FromRoute] int id)
    {
        Appointment? appointment = await _appointmentService.GetAppointmentAsync(id);

        if (appointment is null)
        {
            return NotFound();
        }

        return AppointmentMapper.ToAppointmentDto(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointmentAsync(
        [FromBody] CreateAppointmentDto createAppointmentDto)
    {
        Appointment newAppointment = AppointmentMapper.ToAppointment(createAppointmentDto);

        AppointmentResult result = await _appointmentService.CreateAppointmentAsync(newAppointment);

        if (result.Error is AppointmentError error)
        {
            return HandleAppointmentError(error);
        }

        if (result.Appointment is null)
        {
            return Problem("Appointment result is missing.");
        }

        return CreatedAtAction(
            nameof(GetAppointmentAsync),
            new { id = result.Appointment.Id },
            AppointmentMapper.ToAppointmentDto(result.Appointment)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointmentAsync(
        [FromRoute] int id,
        [FromBody] UpdateAppointmentDto updateAppointmentDto)
    {
        Appointment appointmentData = AppointmentMapper.ToAppointment(updateAppointmentDto);

        AppointmentResult result = await _appointmentService.UpdateAppointmentAsync(id, appointmentData);

        if (result.Error is AppointmentError error)
        {
            return HandleAppointmentError(error);
        }

        return NoContent();
    }

    // Additional operations
    [HttpGet("/api/patients/{patientId}/appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetPatientAppointmentsAsync(
        [FromRoute] int patientId)
    {
        List<Appointment>? appointments = await _appointmentService.GetPatientAppointmentsAsync(patientId);

        if (appointments is null)
        {
            return NotFound("Patient not found.");
        }

        return appointments
            .Select(AppointmentMapper.ToAppointmentDto)
            .ToList();
    }

    [HttpGet("/api/doctors/{doctorId}/appointments")]
    public async Task<ActionResult<List<AppointmentDto>>> GetDoctorAppointmentsAsync(
        [FromRoute] int doctorId)
    {
        List<Appointment>? appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctorId);

        if (appointments is null)
        {
            return NotFound("Doctor not found.");
        }

        return appointments
            .Select(AppointmentMapper.ToAppointmentDto)
            .ToList();
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<AppointmentDetailsDto>> GetAppointmentDetailsAsync(
        [FromRoute] int id)
    {
        var appointment = await _appointmentService.GetAppointmentDetailsAsync(id);

        if (appointment is null)
        {
            return NotFound();
        }

        return AppointmentMapper.ToAppointmentDetailsDto(appointment);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointmentStatusAsync(
        [FromRoute] int id,
        [FromQuery] AppointmentStatus status)
    {
        AppointmentResult result = await _appointmentService.UpdateAppointmentStatusAsync(id, status);

        if (result.Error is AppointmentError error)
        {
            return HandleAppointmentError(error);
        }

        if (result.Appointment is null)
        {
            return Problem("Appointment result is missing.");
        }

        return AppointmentMapper.ToAppointmentDto(result.Appointment);
    }

    // Private helper method
    private ActionResult HandleAppointmentError(AppointmentError error)
    {
        return error switch
        {
            AppointmentError.AppointmentNotFound =>
                NotFound("Appointment not found."),

            AppointmentError.PatientNotFound =>
                NotFound("Patient not found."),

            AppointmentError.DoctorNotFound =>
                NotFound("Doctor not found."),

            AppointmentError.PatientInactive =>
                Conflict("Patient is inactive."),

            AppointmentError.DoctorInactive =>
                Conflict("Doctor is inactive."),

            AppointmentError.InvalidDate =>
                BadRequest("Appointment date must be in the future."),

            AppointmentError.InvalidDuration =>
                BadRequest("Appointment duration must be greater than zero."),

            AppointmentError.TimeConflict =>
                Conflict("Doctor already has an overlapping appointment."),

            AppointmentError.EditNotAllowed =>
                Conflict("Only future scheduled appointments can be edited."),

            AppointmentError.StatusChangeNotAllowed =>
                Conflict("Appointment status change is not allowed."),

            _ => Problem(
                title: "Unexpected appointment error.",
                statusCode: 500)
        };
    }
}