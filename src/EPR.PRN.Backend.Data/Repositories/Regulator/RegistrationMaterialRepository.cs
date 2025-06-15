using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegistrationMaterialRepository(EprContext eprContext) : IRegistrationMaterialRepository
{
    public async Task<Registration> GetRegistrationById(Guid registrationId)
    {
        var registrations = GetRegistrationsWithRelatedEntities();

        return await registrations.SingleOrDefaultAsync(r => r.ExternalId == registrationId)
               ?? throw new KeyNotFoundException("Registration not found.");
    }

    public async Task<Registration> GetRegistrationByExternalIdAndYear(Guid externalId, int? year)
    {
        var registrations = GetRegistrationsWithRelatedEntitiesAndAccreditations(year);

        return await registrations.SingleOrDefaultAsync(r => r.ExternalId == externalId)
              ?? throw new KeyNotFoundException("Registration not found.");
    }

    public async Task<List<LookupRegulatorTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId) =>
        await eprContext.LookupTasks
            .Where(t => t.ApplicationTypeId == applicationTypeId && t.IsMaterialSpecific == isMaterialSpecific && t.JourneyTypeId == journeyTypeId)
            .ToListAsync();

    public async Task<RegistrationMaterial> GetRegistrationMaterialById(Guid registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterialsWithRelatedEntities()
            .Include(rm => rm.DulyMade)
            .Include(rm => rm.DeterminationDate);

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task<RegistrationMaterial> GetRegistrationMaterial_FileUploadById(Guid registrationMaterialId)
    {
        var registrationMaterials = GetRegistrationMaterial_FileUploadById();

        return await registrationMaterials.SingleOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId)
               ?? throw new KeyNotFoundException("Material not found.");
    }

    public async Task<Accreditation> GetAccreditation_FileUploadById(Guid accreditationId)
    {
        var accreditations = GetAccreditation_FileUploadById();
        return await accreditations.SingleOrDefaultAsync(rm => rm.ExternalId == accreditationId)
               ?? throw new KeyNotFoundException("Accreditation not found.");
    }

    public async Task UpdateRegistrationOutCome(Guid registrationMaterialId, int statusId, string? comment, string registrationReferenceNumber, Guid User)
    {
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);
        if (material is null) throw new KeyNotFoundException("Material not found.");

        material.StatusId = statusId;
        material.Comments = comment;
        material.RegistrationReferenceNumber = registrationReferenceNumber;
        material.StatusUpdatedDate = DateTime.UtcNow;
        material.StatusUpdatedBy = User;

        await eprContext.SaveChangesAsync();
    }
    public async Task RegistrationMaterialsMarkAsDulyMade(
    Guid registrationMaterialId,
    int statusId,
    DateTime determinationDateUtc,
    DateTime dulyMadeDateUtc,
    Guid dulyMadeBy)
    {
        var material = await eprContext.RegistrationMaterials
            .FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);

        if (material is null)
            throw new KeyNotFoundException("Material not found.");

        var registration = await eprContext.Registrations
            .FirstOrDefaultAsync(x => x.Id == material.RegistrationId);

        if (registration is null)
            throw new KeyNotFoundException("Registration not found.");

        // Get or create DulyMade
        var dulyMade = await eprContext.DulyMade
            .FirstOrDefaultAsync(dm => dm.RegistrationMaterial!.ExternalId == registrationMaterialId)
            ?? new DulyMade
            {
                RegistrationMaterialId = material.Id,
                ExternalId = Guid.NewGuid()
            };

        // Get or create DeterminationDate
        var determinationDate = await eprContext.DeterminationDate
            .FirstOrDefaultAsync(dd => dd.RegistrationMaterialId == material.Id)
            ?? new DeterminationDate
            {
                RegistrationMaterialId = material.Id,
                ExternalId = Guid.NewGuid()
            };

        // Get task ID
        var taskId = await eprContext.LookupTasks
            .Where(t => t.Name == RegulatorTaskNames.CheckRegistrationStatus && t.ApplicationTypeId == registration.ApplicationTypeId)
            .Select(t => t.Id)
            .FirstOrDefaultAsync();

        if (taskId == 0)
            throw new InvalidOperationException("CheckRegistrationStatus task not found for the given application type.");

        // Get or create RegulatorApplicationTaskStatus
        var taskStatus = await eprContext.RegulatorApplicationTaskStatus
            .FirstOrDefaultAsync(ts => ts.RegistrationMaterialId == material.Id && ts.RegulatorTaskId == taskId);

        if (taskStatus is null)
        {
            taskStatus = new RegulatorApplicationTaskStatus
            {
                RegistrationMaterialId = material.Id,
                TaskStatusId = statusId,
                ExternalId = Guid.NewGuid(),
                RegulatorTaskId = taskId,
                StatusCreatedDate = DateTime.UtcNow,
                StatusCreatedBy = dulyMadeBy
            };
            await eprContext.RegulatorApplicationTaskStatus.AddAsync(taskStatus);
        }
        else
        {
            taskStatus.TaskStatusId = statusId;
            taskStatus.StatusUpdatedBy = dulyMadeBy;
            taskStatus.StatusUpdatedDate = DateTime.UtcNow;
        }

        // Set common fields
        dulyMade.DulyMadeDate = dulyMadeDateUtc;
        dulyMade.DulyMadeBy = dulyMadeBy;
        determinationDate.DeterminateDate = determinationDateUtc;

        // Add new entities if needed
        if (dulyMade.Id == 0)
            await eprContext.DulyMade.AddAsync(dulyMade);

        if (determinationDate.Id == 0)
            await eprContext.DeterminationDate.AddAsync(determinationDate);

        await eprContext.SaveChangesAsync();
    }

    public async Task CreateRegistrationMaterialWithExemptionsAsync(
    RegistrationMaterial registrationMaterial,
    List<MaterialExemptionReference> exemptionReferences)
    {
        await eprContext.RegistrationMaterials.AddAsync(registrationMaterial);

        foreach (var exemption in exemptionReferences)
        {            
            exemption.RegistrationMaterial = registrationMaterial;
        }

        await eprContext.MaterialExemptionReferences.AddRangeAsync(exemptionReferences);
        await eprContext.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(int registrationId, string material)
    {
        var existingRegistration = await eprContext.Registrations.FindAsync(registrationId);
        if (existingRegistration == null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        var newMaterial = new RegistrationMaterial
        {
            RegistrationId = registrationId,
            Material = await eprContext.LookupMaterials.SingleAsync(m => m.MaterialName == material),
            StatusId = (await eprContext.LookupRegistrationMaterialStatuses.SingleAsync(s => s.Name == "ReadyToSubmit")).Id,
            CreatedDate = DateTime.UtcNow,
            ExternalId = Guid.NewGuid(),
            StatusUpdatedDate = DateTime.UtcNow,
            EnvironmentalPermitWasteManagementTonne = 0,
            InstallationReprocessingTonne = 0,
            WasteManagementReprocessingCapacityTonne = 0,
            PPCReprocessingCapacityTonne = 0,
            IsMaterialRegistered = false,
            // Temp as we need to think about either the journey or the data model as currently we can't insert nulls into the db for this column.
            PermitType = await eprContext.LookupMaterialPermit.SingleAsync(o => o.Name == PermitTypes.WasteManagementLicence)
        };

        await eprContext.RegistrationMaterials.AddAsync(newMaterial);
        await eprContext.SaveChangesAsync();

        return newMaterial.Id;
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
            .Include(rm => rm.PermitType)
            .Include(rm => rm.Registration)
                .ThenInclude(r => r.LegalDocumentAddress)
            .Include(rm => rm.Tasks)!
                .ThenInclude(q => q.ApplicationTaskStatusQueryNotes)!
                .ThenInclude(qn => qn.Note)
            .Include(rm => rm.Tasks)!
                .ThenInclude(q => q.Task)
            .Include(rm => rm.Material)
            .Include(rm => rm.Status);

        return registrationMaterials;
    }

    private IIncludableQueryable<RegistrationMaterial, LookupMaterial> GetRegistrationMaterial_FileUploadById()
    {
        var registrationMaterials =
            eprContext.RegistrationMaterials
                .AsNoTracking()
                .AsSplitQuery()
                .Include(rm => rm.Registration)
                .ThenInclude(r => r.ReprocessingSiteAddress)
                .Include(rm => rm.FileUploads)!
                .ThenInclude(fu => fu.FileUploadType)
                .Include(rm => rm.FileUploads)!
                .ThenInclude(fu => fu.FileUploadStatus)
                .Include(rm => rm.Tasks)!
                .ThenInclude(t => t.Task)
                .Include(rm => rm.Tasks)!
                .ThenInclude(q => q.ApplicationTaskStatusQueryNotes)!
                .ThenInclude(qn => qn.Note)
                .Include(rm => rm.Material);

        return registrationMaterials;
    }

    private IIncludableQueryable<Accreditation, LookupMaterial> GetAccreditation_FileUploadById()
    {
        var accreditations =
            eprContext.Accreditations
            .AsNoTracking()
            .AsSplitQuery()
            .Include(rm => rm.FileUploads)!
            .ThenInclude(fu => fu.FileUploadType)
            .Include(rm => rm.FileUploads)!
            .ThenInclude(fu => fu.FileUploadStatus)
            .Include(rm => rm.RegistrationMaterial)
            .ThenInclude(rm => rm.Material);

        return accreditations;
    }

    private IIncludableQueryable<Registration, LookupRegistrationMaterialStatus> GetRegistrationsWithRelatedEntities()
    {
        var registrations = eprContext
            .Registrations
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.BusinessAddress)
            .Include(r => r.ReprocessingSiteAddress)
            .Include(r => r.CarrierBrokerDealerPermit)
            .Include(r => r.LegalDocumentAddress)
            .Include(r => r.Tasks)!
                .ThenInclude(t => t.TaskStatus)
            .Include(r => r.Tasks)!
                .ThenInclude(t => t.Task)
            .Include(r => r.Tasks)!
                   .ThenInclude(t => t.RegistrationTaskStatusQueryNotes)
                   .ThenInclude(t => t.QueryNote)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Tasks)!
                .ThenInclude(t => t.TaskStatus)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Material)
            .Include(r => r.Materials)!
                .ThenInclude(m => m.Tasks)!
                .ThenInclude(t => t.Task)
            .Include(r => r.Materials)!
                .ThenInclude(rm => rm.Tasks)!
                .ThenInclude(q => q.ApplicationTaskStatusQueryNotes)!
                .ThenInclude(qn => qn.Note)
             .Include(r => r.Materials)!
                .ThenInclude(rm => rm.Status);

        return registrations;
    }

    private IIncludableQueryable<Registration, LookupAccreditationStatus> GetRegistrationsWithRelatedEntitiesAndAccreditations(int? year)
    {
        if (year != null)
        {
            var registrations = eprContext
                .Registrations
                .AsNoTracking()
                .AsSplitQuery()
                .Include(r => r.BusinessAddress)
                .Include(r => r.ReprocessingSiteAddress)
                .Include(r => r.LegalDocumentAddress)
                .Include(r => r.AccreditationTasks!.Where(at => at.AccreditationYear == year))!
                    .ThenInclude(t => t.TaskStatus)
                .Include(r => r.AccreditationTasks!.Where(at => at.AccreditationYear == year))!
                    .ThenInclude(t => t.Task)

                .Include(r => r.Materials)!
                    .ThenInclude(m => m.Material)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Status)

                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations!.Where(at => at.AccreditationYear == year))!
                        .ThenInclude(a => a.AccreditationDulyMade)

                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations!.Where(at => at.AccreditationYear == year))!
                        .ThenInclude(a => a.Tasks)!
                            .ThenInclude(t => t.Task)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations!.Where(at => at.AccreditationYear == year))!
                        .ThenInclude(a => a.Tasks)!
                            .ThenInclude(t => t.TaskStatus)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations!.Where(at => at.AccreditationYear == year))!
                        .ThenInclude(a => a.AccreditationStatus);

            return registrations;
        }
        else
        {
            var registrations = eprContext
                .Registrations
                .AsNoTracking()
                .AsSplitQuery()
                .Include(r => r.BusinessAddress)
                .Include(r => r.ReprocessingSiteAddress)
                .Include(r => r.LegalDocumentAddress)
                .Include(r => r.AccreditationTasks!)!
                    .ThenInclude(t => t.TaskStatus)
                .Include(r => r.AccreditationTasks!)!
                    .ThenInclude(t => t.Task)
                .Include(r => r.Tasks)!
                   .ThenInclude(t => t.RegistrationTaskStatusQueryNotes)
                   .ThenInclude(t => t.QueryNote)
                .Include(r => r.Materials)!
                    .ThenInclude(m => m.Material)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Status)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations)!
                        .ThenInclude(a => a.AccreditationDulyMade)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations)!
                        .ThenInclude(a => a.Tasks)!
                            .ThenInclude(t => t.Task)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations)!
                        .ThenInclude(a => a.Tasks)!
                            .ThenInclude(t => t.TaskStatus)
                  .Include(r => r.Materials)!
                        .ThenInclude(rm => rm.Tasks)!
                            .ThenInclude(q => q.ApplicationTaskStatusQueryNotes)!
                            .ThenInclude(qn => qn.Note)
                .Include(r => r.Materials)!
                    .ThenInclude(rm => rm.Accreditations)!
                        .ThenInclude(a => a.AccreditationStatus);

            return registrations;
        }
    }
}