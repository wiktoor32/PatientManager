using Microsoft.AspNetCore.Mvc;
using PatientManager.Api.Dtos;
using PatientManager.Api.Models;
using PatientManager.Api.Services;

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
            .Select(MapToPatientDto)
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

        return MapToPatientDto(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> CreatePatientAsync(
        [FromBody] CreatePatientDto createPatientDto)
    {
        Patient newPatient = new Patient
        {
            FirstName = createPatientDto.FirstName,
            LastName = createPatientDto.LastName,
            DateOfBirth = createPatientDto.DateOfBirth,
            Email = createPatientDto.Email
        };

        Patient createdPatient = await _patientService.CreatePatientAsync(newPatient);

        return CreatedAtAction(
            nameof(GetPatientAsync),
            new { id = createdPatient.Id },
            MapToPatientDto(createdPatient)
        );
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatientAsync(
        [FromRoute] int id,
        [FromBody] UpdatePatientDto updatePatientDto)
    {
        Patient patientData = new Patient
        {
            FirstName = updatePatientDto.FirstName,
            LastName = updatePatientDto.LastName,
            DateOfBirth = updatePatientDto.DateOfBirth,
            Email = updatePatientDto.Email
        };
        
        Patient? updatedPatient = await _patientService.UpdatePatientAsync(id, patientData);

        if (updatedPatient is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatientAsync(
        [FromRoute] int id)
    {
        if (await _patientService.DeletePatientAsync(id))
        {
            return NoContent();
        }

        return NotFound();
    }
    
    //Additional operations
    [HttpGet("search")]
    public async Task<ActionResult<List<PatientDto>>> SearchByLastNameAsync(
        [FromQuery] string lastName)
    {
        List<Patient> patients = await _patientService.SearchByLastNameAsync(lastName);

        return patients
            .Select(MapToPatientDto)
            .ToList();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<PatientDto>>> FilterByActiveStatusAsync(
        [FromQuery] bool active)
    {
        List<Patient> patients = await _patientService.FilterByActiveStatusAsync(active);

        return patients
            .Select(MapToPatientDto)
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

        return MapToPatientDto(updatedPatient);
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

        PatientSummaryDto patientSummaryDto = new PatientSummaryDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            IsActive = patient.IsActive
        };

        return patientSummaryDto;
    }
    
    private static PatientDto MapToPatientDto(Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Email = patient.Email,
            IsActive = patient.IsActive
        };
    }
}