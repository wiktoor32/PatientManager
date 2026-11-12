using Microsoft.AspNetCore.Mvc;
using PatientManager.Api.Dtos.EmergencyContacts;
using PatientManager.Api.Helpers;
using PatientManager.Api.Mappings;
using PatientManager.Api.Models;
using PatientManager.Api.Services.EmergencyContacts;

namespace PatientManager.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId}/emergency-contact")]
public class EmergencyContactsController : ControllerBase
{
    private readonly IEmergencyContactService _emergencyContactService;

    public EmergencyContactsController(IEmergencyContactService emergencyContactService)
    {
        _emergencyContactService = emergencyContactService;
    }

    [HttpGet]
    [ActionName(nameof(GetEmergencyContactAsync))]
    public async Task<ActionResult<EmergencyContactDto>> GetEmergencyContactAsync(
        [FromRoute] int patientId)
    {
        try
        {
            EmergencyContact? contact = await _emergencyContactService.GetEmergencyContactAsync(patientId);

            if (contact is null)
            {
                return NotFound();
            }

            return EmergencyContactMapper.ToEmergencyContactDto(contact);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<EmergencyContactDto>> CreateEmergencyContactAsync(
        [FromRoute] int patientId,
        [FromBody] CreateEmergencyContactDto createEmergencyContactDto)
    {
        if (!PhoneNumberHelper.TryNormalize(
                createEmergencyContactDto.PhoneNumber,
                out string? normalizedPhone) || normalizedPhone is null)
        {
            return BadRequest("Invalid phone number.");
        }

        EmergencyContact newContact = EmergencyContactMapper.ToEmergencyContact(createEmergencyContactDto, patientId, normalizedPhone);

        try
        {
            EmergencyContact? createdContact = await _emergencyContactService.CreateEmergencyContactAsync(newContact);

            if (createdContact is null)
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(GetEmergencyContactAsync),
                new { patientId = createdContact.PatientId },
                EmergencyContactMapper.ToEmergencyContactDto(createdContact)
            );
        }
        catch (InvalidOperationException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmergencyContactAsync(
        [FromRoute] int patientId,
        [FromBody] UpdateEmergencyContactDto updateEmergencyContactDto)
    {
        if (!PhoneNumberHelper.TryNormalize(
                updateEmergencyContactDto.PhoneNumber,
                out string? normalizedPhone) || normalizedPhone is null)
        {
            return BadRequest("Invalid phone number.");
        }

        EmergencyContact contactData = EmergencyContactMapper.ToEmergencyContact(updateEmergencyContactDto, normalizedPhone);

        try
        {
            EmergencyContact? updatedContact = await _emergencyContactService.UpdateEmergencyContactAsync(patientId, contactData);

            if (updatedContact is null)
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteEmergencyContactAsync(
        [FromRoute] int patientId)
    {
        try
        {
            bool? deleted = await _emergencyContactService.DeleteEmergencyContactAsync(patientId);

            if (deleted is null)
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }

        return NoContent();
    }
}