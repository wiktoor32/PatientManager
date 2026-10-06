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
    public ActionResult<List<PatientDto>> GetPatients()
    {
        return _patientService
            .GetPatients()
            .Select(MapToPatientDto)
            .ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<PatientDto> GetPatient(
        [FromRoute] int id)
    {
        Patient? patient = _patientService.GetPatient(id);

        if (patient is null)
        {
            return NotFound();
        }

        return MapToPatientDto(patient);
    }

    [HttpPost]
    public ActionResult<PatientDto> CreatePatient(
        [FromBody] CreatePatientDto createPatientDto)
    {
        Patient newPatient = new Patient
        {
            FirstName = createPatientDto.FirstName,
            LastName = createPatientDto.LastName,
            DateOfBirth = createPatientDto.DateOfBirth,
            Email = createPatientDto.Email,
        };

        Patient createdPatient = _patientService.CreatePatient(newPatient);

        return CreatedAtAction(
            nameof(GetPatient),
            new { id = createdPatient.Id },
            MapToPatientDto(createdPatient)
        );
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdatePatient(
        [FromRoute] int id,
        [FromBody] UpdatePatientDto updatePatientDto)
    {
        Patient patientData = new Patient
        {
            FirstName = updatePatientDto.FirstName,
            LastName = updatePatientDto.LastName,
            DateOfBirth = updatePatientDto.DateOfBirth,
            Email = updatePatientDto.Email,
            IsActive = updatePatientDto.IsActive
        };
        
        Patient? updatedPatient = _patientService.UpdatePatient(id, patientData);

        if (updatedPatient is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePatient(
        [FromRoute] int id)
    {
        if (_patientService.DeletePatient(id))
        {
            return NoContent();
        }

        return NotFound();
    }
    
    //Additional operations
    [HttpGet("search")]
    public ActionResult<List<PatientDto>> SearchByLastName(
        [FromQuery] string lastName)
    {
        return _patientService
            .SearchByLastName(lastName)
            .Select(MapToPatientDto)
            .ToList();
    }

    [HttpGet("filter")]
    public ActionResult<List<PatientDto>> FilterByActiveStatus(
        [FromQuery] bool active)
    {
        return _patientService
            .FilterByActiveStatus(active)
            .Select(MapToPatientDto)
            .ToList();
    }

    [HttpPut("{id}/status")]
    public ActionResult<PatientDto> UpdateActiveStatus(
        [FromRoute] int id,
        [FromQuery] bool active)
    {
        Patient? updatedPatient = _patientService.UpdateActiveStatus(id, active);

        if (updatedPatient is null)
        {
            return NotFound();
        }

        return MapToPatientDto(updatedPatient);
    }

    [HttpGet("{id}/summary")]
    public ActionResult<PatientSummaryDto> GetPatientSummary(
        [FromRoute] int id)
    {
        Patient? patient = _patientService.GetPatient(id);

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