using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegistrationMaterialRepository(EprContext eprContext) : IRegistrationMaterialRepository
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

    public async Task<RegistrationMaterial> GetRegistrationMaterial_WasteLicencesById(int registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterialsWithRelatedEntities_WasteLicences();

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.Id == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task<RegistrationMaterial> GetRegistrationMaterial_RegistrationReprocessingIOById(int registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterialsWithRelatedEntities_RegistrationReprocessingIO();

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.Id == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(int registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterial_FileUploadById();

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.Id == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task UpdateRegistrationOutCome(int registrationMaterialId, int statusId, string? comment, string registrationReferenceNumber)
    {
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");

        material.StatusId = statusId;
        material.Comments = comment;
        material.RegistrationReferenceNumber = registrationReferenceNumber;
        material.StatusUpdatedDate = DateTime.UtcNow;
        material.StatusUpdatedBy = Guid.NewGuid();

        await eprContext.SaveChangesAsync();
    }
    public async Task RegistrationMaterialsMarkAsDulyMade(int registrationMaterialId, int statusId, DateTime DeterminationDate, DateTime DulyMadeDate, Guid DulyMadeBy)
    {
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");
        var dulyMade = await eprContext.DulyMade
            .FirstOrDefaultAsync(rm => rm.RegistrationMaterialId == registrationMaterialId)
            ?? new DulyMade
            {
                RegistrationMaterialId = registrationMaterialId,
                RegistrationMaterial = material // Initialize the required member
            };

        var registration = await eprContext.Registrations
     .FirstOrDefaultAsync(x => x.Id == material.RegistrationId);

        if (registration == null)
            throw new KeyNotFoundException("Registration not found.");

        var applicationTypeId = registration.ApplicationTypeId;


        var taskid = await eprContext.LookupTasks
            .Where(t => t.Name == "CheckRegistrationStatus" && t.ApplicationTypeId == applicationTypeId)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();
        var regulatorApplicationTaskStatus = new RegulatorApplicationTaskStatus
        {
            RegistrationMaterialId = registrationMaterialId,
            TaskStatusId = statusId,
            ExternalId = material.ExternalId,
            TaskId = taskid,
            StatusCreatedDate = DateTime.UtcNow,
            StatusUpdatedBy = DulyMadeBy

        };

        // Set/update the fields
        dulyMade.TaskStatusId = statusId;
        dulyMade.DeterminationDate = DeterminationDate;
        dulyMade.DulyMadeDate = DulyMadeDate;
        dulyMade.DulyMadeBy = DulyMadeBy;
        dulyMade.ExternalId = material.ExternalId;

        // If this is a new entity, add it to the context
        if (dulyMade.Id == 0)
        {
            await eprContext.DulyMade.AddAsync(dulyMade);
            await eprContext.RegulatorApplicationTaskStatus.AddAsync(regulatorApplicationTaskStatus);
        }

        await eprContext.SaveChangesAsync();
    }

    private IIncludableQueryable<RegistrationMaterial, LookupRegistrationMaterialStatus> GetRegistrationMaterialsWithRelatedEntities()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.Registration)
                .ThenInclude(r => r.ReprocessingSiteAddress)
            .Include(rm => rm.Registration)
                .ThenInclude(r => r.BusinessAddress)
             .Include(rm => rm.Registration)
                .ThenInclude(r => r.LegalDocumentAddress)
            .Include(rm => rm.Material)
            .Include(rm => rm.Status);

        return registrationMaterials;
    }

    private IIncludableQueryable<RegistrationMaterial, LookupMaterial> GetRegistrationMaterialsWithRelatedEntities_WasteLicences()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.MaterialExemptionReferences)
            .Include(rm => rm.PPCPeriod)
            .Include(rm => rm.WasteManagementPeriod)
            .Include(rm => rm.InstallationPeriod)
            .Include(rm => rm.EnvironmentalPermitWasteManagementPeriod)
            .Include(rm => rm.MaximumReprocessingPeriod)
            .Include(rm => rm.PermitType)
            .Include(rm => rm.Material);

        return registrationMaterials;
    }

    private IIncludableQueryable<RegistrationMaterial, LookupMaterial> GetRegistrationMaterialsWithRelatedEntities_RegistrationReprocessingIO()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.RegistrationReprocessingIO)
            .Include(rm => rm.Material);

        return registrationMaterials;
    }

    private IIncludableQueryable<RegistrationMaterial, LookupMaterial> GetRegistrationMaterial_FileUploadById()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.FileUploads)!
            .ThenInclude(fu => fu.FileUploadType)
            .Include(rm => rm.FileUploads)!
            .ThenInclude(fu => fu.FileUploadStatus)
            .Include(rm => rm.Material);

        return registrationMaterials;
    }

    private IIncludableQueryable<Registration, LookupRegistrationMaterialStatus> GetRegistrationsWithRelatedEntities()
    {
        var registrations = eprContext
            .Registrations
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.BusinessAddress)
            .Include(r => r.ReprocessingSiteAddress)
            .Include(r => r.LegalDocumentAddress)
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
    
    public async Task CreateRegistrationMaterialWithExemptionsAsync(
        RegistrationMaterial registrationMaterial,
        List<MaterialExemptionReference> exemptionReferences)
    {        
        await eprContext.RegistrationMaterials.AddAsync(registrationMaterial);
        //await eprContext.SaveChangesAsync();
     
        foreach (var exemption in exemptionReferences)
        {
            exemption.RegistrationMaterialId = registrationMaterial.Id;
            exemption.RegistrationMaterial = registrationMaterial;
        }

        await eprContext.MaterialExemptionReferences.AddRangeAsync(exemptionReferences);
        await eprContext.SaveChangesAsync();       
    }
}