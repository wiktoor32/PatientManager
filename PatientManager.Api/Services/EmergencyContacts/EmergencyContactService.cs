using Microsoft.EntityFrameworkCore;
using PatientManager.Api.Data;
using PatientManager.Api.Models;

namespace PatientManager.Api.Services.EmergencyContacts;

public class EmergencyContactService : IEmergencyContactService
{
    private readonly AppDbContext _context;

    public EmergencyContactService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmergencyContact?> GetEmergencyContactAsync(int patientId)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
        {
            return null;
        }

        EmergencyContact? contact = await _context.EmergencyContacts
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.PatientId == patientId);

        if (contact is null)
        {
            throw new InvalidOperationException("Patient has no emergency contact.");
        }

        return contact;
    }

    public async Task<EmergencyContact?> CreateEmergencyContactAsync(EmergencyContact newContact)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.Id == newContact.PatientId);

        if (!patientExists)
        {
            return null;
        }

        bool contactExists = await _context.EmergencyContacts
            .AnyAsync(e => e.PatientId == newContact.PatientId);

        if (contactExists)
        {
            throw new InvalidOperationException("Patient already has an emergency contact.");
        }

        _context.EmergencyContacts.Add(newContact);

        await _context.SaveChangesAsync();

        return newContact;
    }

    public async Task<EmergencyContact?> UpdateEmergencyContactAsync(int patientId, EmergencyContact contactData)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
        {
            return null;
        }

        EmergencyContact? existingContact = await _context.EmergencyContacts
            .FirstOrDefaultAsync(e => e.PatientId == patientId);

        if (existingContact is null)
        {
            throw new InvalidOperationException("Patient has no emergency contact.");
        }

        existingContact.FirstName = contactData.FirstName;
        existingContact.LastName = contactData.LastName;
        existingContact.Relationship = contactData.Relationship;
        existingContact.PhoneNumber = contactData.PhoneNumber;

        await _context.SaveChangesAsync();

        return existingContact;
    }

    public async Task<bool?> DeleteEmergencyContactAsync(int patientId)
    {
        bool patientExists = await _context.Patients
            .AnyAsync(p => p.Id == patientId);

        if (!patientExists)
        {
            return null;
        }

        EmergencyContact? existingContact = await _context.EmergencyContacts
            .FirstOrDefaultAsync(e => e.PatientId == patientId);

        if (existingContact is null)
        {
            throw new InvalidOperationException("Patient has no emergency contact.");
        }

        _context.EmergencyContacts.Remove(existingContact);

        await _context.SaveChangesAsync();

        return true;
    }
}