using Microsoft.AspNetCore.Mvc;
using PatientManager.Api.Dtos.Doctors;
using PatientManager.Api.Models;
using PatientManager.Api.Mappings;
using PatientManager.Api.Services.Doctors;

namespace PatientManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    //Basic operations
    [HttpGet]
    public async Task<ActionResult<List<DoctorDto>>> GetDoctorsAsync()
    {
        List<Doctor> doctors = await _doctorService.GetDoctorsAsync();

        return doctors
            .Select(DoctorMapper.ToDoctorDto)
            .ToList();
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetDoctorAsync))]
    public async Task<ActionResult<DoctorDto>> GetDoctorAsync(
        [FromRoute] int id)
    {
        Doctor? doctor = await _doctorService.GetDoctorAsync(id);

        if (doctor is null)
        {
            return NotFound();
        }

        return DoctorMapper.ToDoctorDto(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> CreateDoctorAsync(
        [FromBody] CreateDoctorDto createDoctorDto)
    {
        Doctor newDoctor = DoctorMapper.ToDoctor(createDoctorDto);

        Doctor createdDoctor = await _doctorService.CreateDoctorAsync(newDoctor);

        return CreatedAtAction(
            nameof(GetDoctorAsync),
            new { id = createdDoctor.Id },
            DoctorMapper.ToDoctorDto(createdDoctor)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDoctorAsync(
        [FromRoute] int id,
        [FromBody] UpdateDoctorDto updateDoctorDto)
    {
        Doctor doctorData = DoctorMapper.ToDoctor(updateDoctorDto);

        Doctor? updatedDoctor = await _doctorService.UpdateDoctorAsync(id, doctorData);

        if (updatedDoctor is null)
        {
            return NotFound();
        }

        return NoContent();
    }

    // Additional operations
    [HttpGet("search/last-name")]
    public async Task<ActionResult<List<DoctorDto>>> SearchByLastNameAsync(
        [FromQuery] string lastName)
    {
        List<Doctor> doctors = await _doctorService.SearchByLastNameAsync(lastName);

        return doctors
            .Select(DoctorMapper.ToDoctorDto)
            .ToList();
    }

    [HttpGet("search/specialization")]
    public async Task<ActionResult<List<DoctorDto>>> SearchBySpecializationAsync(
        [FromQuery] string specialization)
    {
        List<Doctor> doctors = await _doctorService.SearchBySpecializationAsync(specialization);

        return doctors
            .Select(DoctorMapper.ToDoctorDto)
            .ToList();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<List<DoctorDto>>> FilterByActiveStatusAsync(
        [FromQuery] bool? active)
    {
        if (active is null)
        {
            return BadRequest("Active status is required.");
        }

        List<Doctor> doctors = await _doctorService.FilterByActiveStatusAsync(active.Value);

        return doctors
            .Select(DoctorMapper.ToDoctorDto)
            .ToList();
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctorDetailsAsync(
        [FromRoute] int id)
    {
        Doctor? doctor = await _doctorService.GetDoctorDetailsAsync(id);

        if (doctor is null)
        {
            return NotFound();
        }

        return DoctorMapper.ToDoctorDetailsDto(doctor);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<DoctorDto>> UpdateActiveStatusAsync(
        [FromRoute] int id,
        [FromQuery] bool? active)
    {
        if (active is null)
        {
            return BadRequest("Active status is required.");
        }

        Doctor? updatedDoctor = await _doctorService.UpdateActiveStatusAsync(id, active.Value);

        if (updatedDoctor is null)
        {
            return NotFound();
        }

        return DoctorMapper.ToDoctorDto(updatedDoctor);
    }
}