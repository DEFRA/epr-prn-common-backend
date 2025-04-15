namespace EPR.PRN.Backend.Data.Repositories;

using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
    protected readonly EprRegistrationsContext _eprContext = eprContext;
    public async Task<Registration> GetRegistrationById(int registrationId) =>
        await _eprContext.Registrations.FirstOrDefaultAsync(r => r.Id == registrationId)
        ?? throw new KeyNotFoundException("Registration not found.");

    public async Task<List<RegistrationMaterial>> GetMaterialsByRegistrationId(int registrationId) =>
        await _eprContext.RegistrationMaterials
            .Include(rm => rm.Material)
            .Include(rm => rm.Status)
            .Include(rm => rm.Registration)
            .Where(rm => rm.RegistrationId == registrationId)
            .ToListAsync();

    public async Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific) =>
        await _eprContext.LookupTasks
            .Where(t => t.ApplicationTypeId == applicationTypeId && t.IsMaterialSpecific == isMaterialSpecific && t.JourneyTypeId == 1)
            .ToListAsync();

    public async Task<List<RegulatorRegistrationTaskStatus>> GetRegistrationTasks(int registrationId) =>
        await _eprContext.RegulatorRegistrationTaskStatus
            .Include(t => t.Task)
            .Include(t => t.TaskStatus)
            .Where(t => t.RegistrationId == registrationId)
            .ToListAsync();

    public async Task<List<RegulatorApplicationTaskStatus>> GetMaterialTasks(int registrationMaterialId) =>
        await _eprContext.RegulatorApplicationTaskStatus
            .Include(t => t.Task)
            .Include(t => t.TaskStatus)
            .Where(t => t.RegistrationMaterialId == registrationMaterialId)
            .ToListAsync();

    public async Task<RegistrationMaterial> GetRegistrationMaterialById(int registrationMaterialId) =>
        await _eprContext.RegistrationMaterials
            .Include(rm => rm.Material)
            .Include(rm => rm.Status)
            .FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId)
            ?? throw new KeyNotFoundException("Material not found.");

    public async Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string registrationReferenceNumber)
    {
        var material = await _eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");

        material.StatusID = statusId;
        material.Comments = comment;
        material.ReferenceNumber = registrationReferenceNumber;

        await _eprContext.SaveChangesAsync();
    }

    public async Task<LookupAddress?> GetAddressById(int addressId) =>
    await _eprContext.LookupAddresses.FirstOrDefaultAsync(a => a.Id == addressId);

    public async Task<LookupMaterial?> GetMaterialById(int materialId) =>
        await _eprContext.LookupMaterials.FirstOrDefaultAsync(m => m.Id == materialId);
}