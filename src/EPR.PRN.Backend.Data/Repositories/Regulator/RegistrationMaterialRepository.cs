using System.Threading.Tasks;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories.Regulator;

public class RegistrationMaterialRepository(ILogger<RegistrationMaterialRepository> logger, EprContext eprContext) : IRegistrationMaterialRepository
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

    public async Task CreateExemptionReferencesAsync(Guid registrationMaterialId, List<MaterialExemptionReference> exemptionReferences)
    {
        var material = await eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId);
        
        if (material is null) throw new KeyNotFoundException("Material not found.");

        foreach (var exemptionReference in exemptionReferences)
        {
            exemptionReference.RegistrationMaterialId = material.Id;
        }

        await eprContext.MaterialExemptionReferences.AddRangeAsync(exemptionReferences);
        await eprContext.SaveChangesAsync();
    }

    public async Task<IList<RegistrationMaterial>> GetRegistrationMaterialsByRegistrationId(Guid registrationId)
    {
        var existingRegistration = await eprContext.Registrations.SingleOrDefaultAsync(o => o.ExternalId == registrationId);
        if (existingRegistration == null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        var existingMaterials = eprContext.RegistrationMaterials
            .AsNoTracking()
            .Include(o => o.PermitType)
            .Include(o => o.Status)
            .Include(o => o.Material)
            .Include(o => o.Registration)
            .Include(o => o.PPCPeriod)
            .Include(o => o.InstallationPeriod)
            .Include(o => o.WasteManagementPeriod)
            .Include(o => o.EnvironmentalPermitWasteManagementPeriod)
            .Include(o => o.MaximumReprocessingPeriod)
            .Include(o => o.MaterialExemptionReferences)
            .Where(o => o.Registration.ExternalId == registrationId)
            .ToList();

        return existingMaterials;
    }

    public async Task<RegistrationMaterial> CreateAsync(Guid registrationId, string material)
    {
        var existingRegistration = await eprContext.Registrations.SingleOrDefaultAsync(o => o.ExternalId == registrationId);
        if (existingRegistration == null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        var existingMaterial = await eprContext.RegistrationMaterials
            .Include(o => o.Material)
            .Include(o => o.Registration)
            .SingleOrDefaultAsync(o => o.Material.MaterialName == material && o.Registration.ExternalId == registrationId);

        if (existingMaterial is not null)
        {
            return existingMaterial;
        }

        var newMaterial = new RegistrationMaterial
        {
            RegistrationId = existingRegistration.Id,
            Material = await eprContext.LookupMaterials.SingleAsync(m => m.MaterialName == material),
            StatusId = (await eprContext.LookupRegistrationMaterialStatuses.SingleAsync(s => s.Name == "ReadyToSubmit")).Id,
            CreatedDate = DateTime.UtcNow,
            ExternalId = Guid.NewGuid(),
            StatusUpdatedDate = DateTime.UtcNow,
            IsMaterialRegistered = false
        };

        await eprContext.RegistrationMaterials.AddAsync(newMaterial);
        await eprContext.SaveChangesAsync();

        return newMaterial;
    }

    public async Task UpdateRegistrationMaterialPermits(Guid registrationMaterialId, int permitTypeId, string? permitNumber)
    {
        var registrationMaterial = await eprContext.RegistrationMaterials
                                        .FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId) ?? throw new KeyNotFoundException("Material not found.");

        // Permit Type Id
        registrationMaterial.PermitTypeId = permitTypeId;

        // Permit Number
        switch ((MaterialPermitType)permitTypeId)
        {
            case MaterialPermitType.PollutionPreventionAndControlPermit:
                registrationMaterial.PPCPermitNumber = permitNumber;
                break;
            case MaterialPermitType.WasteManagementLicence:
                registrationMaterial.WasteManagementLicenceNumber = permitNumber;
                break;
            case MaterialPermitType.InstallationPermit:
                registrationMaterial.InstallationPermitNumber = permitNumber;
                break;
            case MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence:
                registrationMaterial.EnvironmentalPermitWasteManagementNumber = permitNumber;
                break;
        }

        await eprContext.SaveChangesAsync();
    }

    public async Task UpdateRegistrationMaterialPermitCapacity(Guid registrationMaterialId, int permitTypeId, decimal? capacityInTonnes, int? periodId)
    {
        var registrationMaterial = await eprContext.RegistrationMaterials
                                        .FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId) ?? throw new KeyNotFoundException("Material not found.");

        // Capacity in tonnes and period Ids
        switch ((MaterialPermitType)permitTypeId)
        {
            case MaterialPermitType.PollutionPreventionAndControlPermit:
                registrationMaterial.PPCReprocessingCapacityTonne = capacityInTonnes ?? 0;
                registrationMaterial.PPCPeriodId = periodId;
                break;
            case MaterialPermitType.WasteManagementLicence:
                registrationMaterial.WasteManagementReprocessingCapacityTonne = capacityInTonnes ?? 0;
                registrationMaterial.WasteManagementPeriodId = periodId;
                break;
            case MaterialPermitType.InstallationPermit:
                registrationMaterial.InstallationReprocessingTonne = capacityInTonnes ?? 0;
                registrationMaterial.InstallationPeriodId = periodId;
                break;
            case MaterialPermitType.EnvironmentalPermitOrWasteManagementLicence:
                registrationMaterial.EnvironmentalPermitWasteManagementTonne = capacityInTonnes ?? 0;
                registrationMaterial.EnvironmentalPermitWasteManagementPeriodId = periodId;
                break;
        }

        await eprContext.SaveChangesAsync();
    }

    public async Task UpdateMaximumWeightForSiteAsync(Guid registrationMaterialId, decimal weightInTonnes, int periodId)
    {
        var registrationMaterial = await eprContext.RegistrationMaterials
            .FirstOrDefaultAsync(rm => rm.ExternalId == registrationMaterialId) ?? throw new KeyNotFoundException("Material not found.");

        registrationMaterial.MaximumReprocessingPeriodId = periodId;
        registrationMaterial.MaximumReprocessingCapacityTonne = weightInTonnes;
        registrationMaterial.IsMaterialRegistered = true;

        await eprContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<LookupMaterialPermit>> GetMaterialPermitTypes()
    {
        return await eprContext.LookupMaterialPermit
                            .AsNoTracking()
                            .ToListAsync();
    }

    public async Task DeleteAsync(Guid registrationMaterialId)
    {
        var existing =
            await eprContext.RegistrationMaterials.SingleOrDefaultAsync(o => o.ExternalId == registrationMaterialId);

        if (existing is null)
        {
            throw new KeyNotFoundException("Registration material not found.");
        }

        var existingRegistrationStatus = await eprContext.RegistrationTaskStatus.FirstOrDefaultAsync(o => o.RegistrationMaterialId == existing.Id);
        if (existingRegistrationStatus is not null)
        {
            eprContext.RegistrationTaskStatus.Remove(existingRegistrationStatus);
        }

        eprContext.RegistrationMaterials.Remove(existing);

        await eprContext.SaveChangesAsync();
	}

	public async Task UpdateIsMaterialRegisteredAsync(List<UpdateIsMaterialRegisteredDto> updateIsMaterialRegisteredDto)
	{
        foreach (var registrationMaterial in updateIsMaterialRegisteredDto)
        {
			var existing =
			await eprContext.RegistrationMaterials.SingleOrDefaultAsync(o => o.ExternalId == registrationMaterial.RegistrationMaterialId);

			if (existing is null)
			{
				throw new KeyNotFoundException("Registration material not found.");
			}

            existing.IsMaterialRegistered = registrationMaterial.IsMaterialRegistered!.Value;
            existing.StatusId = (int)RegistrationMaterialStatus.InProgress;

			eprContext.RegistrationMaterials.Update(existing);
		}

		await eprContext.SaveChangesAsync();
	}

    public async Task UpdateRegistrationTaskStatusAsync(string taskName, Guid registrationMaterialId, TaskStatuses status)
    {
        logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", taskName, registrationMaterialId, status);

        var registrationMaterial = await eprContext.RegistrationMaterials
            .Include(o => o.Registration)
            .FirstOrDefaultAsync(o => o.ExternalId == registrationMaterialId);

        if (registrationMaterial is null)
        {
            throw new KeyNotFoundException();
        }

        var statusEntity = await eprContext.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

        var taskStatus = await GetTaskStatusAsync(taskName, registrationMaterial.Id);
        if (taskStatus is null)
        {
            var task = await eprContext
                .LookupApplicantRegistrationTasks
                .SingleOrDefaultAsync(t => t.Name == taskName && t.IsMaterialSpecific && t.ApplicationTypeId == registrationMaterial.Registration.ApplicationTypeId);

            if (task is null)
            {
                throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
            }

            // Create a new entity if it doesn't exist
            taskStatus = new ApplicantRegistrationTaskStatus
            {
                ExternalId = Guid.NewGuid(),
                RegistrationId = null,
                RegistrationMaterialId = registrationMaterial.Id,
                Task = task,
                TaskStatus = statusEntity,
            };

            await eprContext.RegistrationTaskStatus.AddAsync(taskStatus);
        }
        else
        {
            // Update the existing entity
            taskStatus.TaskStatus = statusEntity;

            eprContext.RegistrationTaskStatus.Update(taskStatus);
        }
        await eprContext.SaveChangesAsync();

        logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationMaterialId {RegistrationMaterialId} to {Status}", taskName, registrationMaterialId, status);
    }

    public async Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationMaterialId)
    {
        var taskStatus = await eprContext
            .RegistrationTaskStatus
            .Include(ts => ts.TaskStatus)
            .Include(o => o.RegistrationMaterial)
            .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.RegistrationMaterialId == registrationMaterialId);

        return taskStatus;
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