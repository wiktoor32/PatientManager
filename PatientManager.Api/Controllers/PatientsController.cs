using Microsoft.AspNetCore.Mvc;
using PatientManager.Api.Dtos;
using PatientManager.Api.Models;
using PatientManager.Api.Services;
using PatientManager.Api.Helpers;
using PatientManager.Api.Mappings;

namespace PatientManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    //Basic operations
    [HttpGet]
    public async Task<ActionResult<List<PatientDto>>> GetPatientsAsync()
    {
        List<Patient> patients = await _patientService.GetPatientsAsync();

        return patients
            .Select(PatientMapper.ToPatientDto)
            .ToList();
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetPatientAsync))]
    public async Task<ActionResult<PatientDto>> GetPatientAsync(
        [FromRoute] int id)
    {
        Patient? patient = await _patientService.GetPatientAsync(id);

        if (patient is null)
        {
            return NotFound();
        }

        return PatientMapper.ToPatientDto(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatientAsync(
        [FromBody] CreatePatientDto createPatientDto)
    {
        if (!PhoneNumberHelper.TryNormalize(
                createPatientDto.PhoneNumber,
                out string? normalizedPhone) || normalizedPhone is null)
        {
            return BadRequest("Invalid phone number.");
        }

        Patient newPatient = PatientMapper.ToPatient(createPatientDto, normalizedPhone);

        Patient createdPatient = await _patientService.CreatePatientAsync(newPatient);

        return CreatedAtAction(
            nameof(GetPatientAsync),
            new { id = createdPatient.Id },
            PatientMapper.ToPatientDto(createdPatient)
        );
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatientAsync(
        [FromRoute] int id,
        [FromBody] UpdatePatientDto updatePatientDto)
    {
        if (!PhoneNumberHelper.TryNormalize(
                updatePatientDto.PhoneNumber,
                out string? normalizedPhone) || normalizedPhone is null)
        {
            return BadRequest("Invalid phone number.");
        }

        Patient patientData = PatientMapper.ToPatient(updatePatientDto, normalizedPhone);
        
        Patient? updatedPatient = await _patientService.UpdatePatientAsync(id, patientData);

        if (updatedPatient is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    //Additional operations
    [HttpGet("search")]
    public async Task<ActionResult<List<PatientDto>>> SearchByLastNameAsync(
        [FromQuery] string lastName)
    {
        List<Patient> patients = await _patientService.SearchByLastNameAsync(lastName);

        return patients
            .Select(PatientMapper.ToPatientDto)
            .ToList();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<PatientDto>>> FilterByActiveStatusAsync(
        [FromQuery] bool active)
    {
        List<Patient> patients = await _patientService.FilterByActiveStatusAsync(active);

        return patients
            .Select(PatientMapper.ToPatientDto)
            .ToList();
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<PatientDto>> UpdateActiveStatusAsync(
        [FromRoute] int id,
        [FromQuery] bool active)
    {
        Patient? updatedPatient = await _patientService.UpdateActiveStatusAsync(id, active);

        if (updatedPatient is null)
        {
            return NotFound();
        }

        return PatientMapper.ToPatientDto(updatedPatient);
    }

    [HttpGet("{id}/summary")]
    public async Task<ActionResult<PatientSummaryDto>> GetPatientSummaryAsync(
        [FromRoute] int id)
    {
        Patient? patient = await _patientService.GetPatientAsync(id);

        if (patient is null)
        {
            return NotFound();
        }

        return PatientMapper.ToPatientSummaryDto(patient);
    }
}