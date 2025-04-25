using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
    public async Task<Registration> GetRegistrationById(int registrationId)
    {
        var registrations = GetRegistrationsWithRelatedEntities();

        return await registrations.SingleOrDefaultAsync(r => r.Id == registrationId)
               ?? throw new KeyNotFoundException("Registration not found.");
    }

    public async Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific) =>
        await eprContext.LookupTasks
            .Where(t => t.ApplicationTypeId == applicationTypeId && t.IsMaterialSpecific == isMaterialSpecific && t.JourneyTypeId == 1)
            .ToListAsync();

    public async Task<RegistrationMaterial> GetRegistrationMaterialById(int registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterialsWithRelatedEntities();

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.Id == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string? registrationReferenceNumber)
    {
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");

        material.StatusID = statusId;
        material.Comments = comment;
        material.ReferenceNumber = registrationReferenceNumber;
        material.StatusUpdatedDate = DateTime.UtcNow;
        material.StatusUpdatedBy = "Test User";

        await eprContext.SaveChangesAsync();
    }

    private IIncludableQueryable<RegistrationMaterial, LookupMaterialPermit> GetRegistrationMaterialsWithRelatedEntities()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.Registration)
                .ThenInclude(r => r.ReprocessingSiteAddress)
            .Include(rm => rm.Registration)
                .ThenInclude(r => r.BusinessAddress)
            .Include(rm => rm.Material)
            .Include(rm => rm.Status)
            .Include(rm => rm.MaterialExemptionReferences)
            .Include(rm => rm.PPCPeriod)
            .Include(rm => rm.WasteManagementPeriod)
            .Include(rm => rm.InstallationPeriod)
            .Include(rm => rm.EnvironmentalPermitWasteManagementPeriod)
            .Include(rm => rm.MaximumReprocessingPeriod)
            .Include(rm => rm.PermitType);
            

        return registrationMaterials;
    }

    private IIncludableQueryable<Registration, LookupRegistrationMaterialStatus> GetRegistrationsWithRelatedEntities()
    {
        var registrations = eprContext
            .Registrations
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.ReprocessingSiteAddress)
            .Include(r => r.Tasks)!
                .ThenInclude(t => t.TaskStatus)
            .Include(r => r.Tasks)!
                .ThenInclude(t => t.Task)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Tasks)!
                .ThenInclude(t => t.TaskStatus)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Material)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Tasks)!
                .ThenInclude(t => t.Task)
            .Include(r => r.Materials)!
                .ThenInclude(rm => rm.Status);
                
        return registrations;
    }
}