using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.DTO.Registration;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.Repositories;

public class RegistrationRepository(EprContext context, ILogger<RegistrationRepository> logger) : IRegistrationRepository
{
    [ExcludeFromCodeCoverage]
    public async Task<Registration?> GetRegistrationByExternalId(Guid externalId, CancellationToken cancellationToken)
    {
        return await context.Registrations
             .AsNoTracking()
             .Where(x => x.ExternalId == externalId)
             .Include(x => x.CarrierBrokerDealerPermit)
             .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Registration> CreateRegistrationAsync(int applicationTypeId, Guid organisationId, AddressDto reprocessingSiteAddress)
    {
        logger.LogInformation("Creating registration for ApplicationTypeId: {ApplicationTypeId} and OrganisationId: {OrganisationId}", applicationTypeId, organisationId);
        var registration = new Registration();

        // Reprocessing Site Address
        if (reprocessingSiteAddress.Id.GetValueOrDefault() == 0)
        {
            var address = new Address
            {
                AddressLine1 = reprocessingSiteAddress.AddressLine1,
                AddressLine2 = reprocessingSiteAddress.AddressLine2,
                TownCity = reprocessingSiteAddress.TownCity,
                County = reprocessingSiteAddress.County,
                PostCode = reprocessingSiteAddress.PostCode,
                NationId = reprocessingSiteAddress.NationId,
                GridReference = reprocessingSiteAddress.GridReference
            };

            await context.LookupAddresses.AddAsync(address);
            await context.SaveChangesAsync();

            registration = new Registration
            {
                ApplicationTypeId = applicationTypeId,
                OrganisationId = organisationId,
                CreatedBy = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                ReprocessingSiteAddressId = address.Id,
                BusinessAddressId = null,
                LegalDocumentAddressId = null,
                AssignedOfficerId = 0,
                CreatedDate = DateTime.UtcNow,
                RegistrationStatusId = 1
            };

            context.Registrations.Add(registration);
        }
        
        await context.SaveChangesAsync();

        logger.LogInformation("Successfully created registration for ApplicationTypeId: {ApplicationTypeId} and OrganisationId: {OrganisationId}", applicationTypeId, organisationId);

        return registration;
    }

    public async Task<Registration?> GetAsync(Guid registrationId)
    {
        var registrations = LoadRegistrationWithRelatedEntities();
        return await registrations
            .SingleOrDefaultAsync(o => o.ExternalId == registrationId);
    }

    public async Task<Registration> GetTasksForRegistrationAndMaterialsAsync(Guid registrationId)
    {
        return await context.Registrations
            .Include(r => r.ApplicantRegistrationTasksStatus)!
            .ThenInclude(s => s.TaskStatus)
            
            .Include(r => r.ApplicantRegistrationTasksStatus)!
            .ThenInclude(s => s.Task)
            
            .Include(r => r.Materials)!
            .ThenInclude(m => m.Material)

            .Include(r => r.Materials)!
            .ThenInclude(m => m.Status)

            .Include(r => r.Materials)!
            .ThenInclude(m => m.ApplicantTaskStatuses)!
            .ThenInclude(o => o.Task)

            .Include(r => r.Materials)!
            .ThenInclude(m => m.ApplicantTaskStatuses)!
            .ThenInclude(o => o.TaskStatus)
            .SingleAsync(r => r.ExternalId == registrationId);
    }

    public async Task<List<LookupApplicantRegistrationTask>> GetRequiredTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId) =>
        await context.LookupApplicantRegistrationTasks
            .Where(t => t.ApplicationTypeId == applicationTypeId && t.IsMaterialSpecific == isMaterialSpecific && t.JourneyTypeId == journeyTypeId)
            .ToListAsync();

    public async Task<Registration?> GetByOrganisationAsync(int applicationTypeId, Guid organisationId)
    {
        var registrations = LoadRegistrationWithRelatedEntities();

        return await registrations
            .Where(o => o.ApplicationTypeId == applicationTypeId)
            .Where(o => o.OrganisationId == organisationId)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, Guid registrationId)
    {
        var taskStatus = await context
            .RegistrationTaskStatus
            .Include(ts => ts.TaskStatus)
            .Include(o => o.Registration)
            .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.Registration.ExternalId == registrationId);

        return taskStatus;
    }

    public async Task UpdateAsync(Guid registrationId, AddressDto businessAddress, AddressDto reprocessingSiteAddress,
        AddressDto legalDocumentsAddress)
    {
        var existing = await context.Registrations
            .Include(o => o.BusinessAddress)
            .Include(o => o.ReprocessingSiteAddress)
            .Include(o => o.LegalDocumentAddress)
            .FirstOrDefaultAsync(o => o.ExternalId == registrationId);

        if (existing is null)
        {
            throw new KeyNotFoundException();
        }

        // Handle Business Address
        if (existing.BusinessAddressId is null)
        {
            // Create new address
            var newBusinessAddress = new Address
            {
                AddressLine1 = businessAddress.AddressLine1,
                AddressLine2 = businessAddress.AddressLine2,
                TownCity = businessAddress.TownCity,
                County = businessAddress.County,
                GridReference = businessAddress.GridReference,
                PostCode = businessAddress.PostCode,
                NationId = businessAddress.NationId
            };

            await context.LookupAddresses.AddAsync(newBusinessAddress);
            await context.SaveChangesAsync();

            existing.BusinessAddressId = newBusinessAddress.Id;
        }
        else
        {
            // Update existing address
            var address = await context.LookupAddresses.FindAsync(existing.BusinessAddressId.Value);
            if (address != null)
            {
                address.AddressLine1 = businessAddress.AddressLine1;
                address.AddressLine2 = businessAddress.AddressLine2;
                address.TownCity = businessAddress.TownCity;
                address.County = businessAddress.County;
                address.GridReference = businessAddress.GridReference;
                address.PostCode = businessAddress.PostCode;
                address.NationId = businessAddress.NationId;
            }
        }

        // Handle Reprocessing Address
        if (existing.ReprocessingSiteAddressId is null)
        {
            // Create new address
            var newReprocessingSiteAddress = new Address
            {
                AddressLine1 = reprocessingSiteAddress.AddressLine1,
                AddressLine2 = reprocessingSiteAddress.AddressLine2,
                TownCity = reprocessingSiteAddress.TownCity,
                County = reprocessingSiteAddress.County,
                GridReference = reprocessingSiteAddress.GridReference,
                PostCode = reprocessingSiteAddress.PostCode,
                NationId = reprocessingSiteAddress.NationId
            };

            await context.LookupAddresses.AddAsync(newReprocessingSiteAddress);
            await context.SaveChangesAsync();

            existing.ReprocessingSiteAddressId = newReprocessingSiteAddress.Id;
        }
        else
        {
            // Update existing address
            var address = await context.LookupAddresses.FindAsync(existing.ReprocessingSiteAddressId.Value);
            if (address != null)
            {
                address.AddressLine1 = reprocessingSiteAddress.AddressLine1;
                address.AddressLine2 = reprocessingSiteAddress.AddressLine2;
                address.TownCity = reprocessingSiteAddress.TownCity;
                address.County = reprocessingSiteAddress.County;
                address.GridReference = reprocessingSiteAddress.GridReference;
                address.PostCode = reprocessingSiteAddress.PostCode;
                address.NationId = reprocessingSiteAddress.NationId;
            }
        }

        // Handle Legal Address
        if (existing.LegalDocumentAddressId is null)
        {
            // Create new address
            var newLegalAddress = new Address
            {
                AddressLine1 = legalDocumentsAddress.AddressLine1,
                AddressLine2 = legalDocumentsAddress.AddressLine2,
                TownCity = legalDocumentsAddress.TownCity,
                County = legalDocumentsAddress.County,
                GridReference = legalDocumentsAddress.GridReference,
                PostCode = legalDocumentsAddress.PostCode,
                NationId = legalDocumentsAddress.NationId
            };

            await context.LookupAddresses.AddAsync(newLegalAddress);
            await context.SaveChangesAsync();

            existing.LegalDocumentAddressId = newLegalAddress.Id;
        }
        else
        {
            // Update existing address
            var address = await context.LookupAddresses.FindAsync(existing.LegalDocumentAddressId.Value);
            if (address != null)
            {
                address.AddressLine1 = legalDocumentsAddress.AddressLine1;
                address.AddressLine2 = legalDocumentsAddress.AddressLine2;
                address.TownCity = legalDocumentsAddress.TownCity;
                address.County = legalDocumentsAddress.County;
                address.GridReference = legalDocumentsAddress.GridReference;
                address.PostCode = legalDocumentsAddress.PostCode;
                address.NationId = legalDocumentsAddress.NationId;
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task UpdateRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status)
    {
        logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", taskName, registrationId, status);

        var statusEntity = await context.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

        var taskStatus = await GetTaskStatusAsync(taskName, registrationId);
        if (taskStatus is null)
        {
            var registration = await context.Registrations.FirstOrDefaultAsync(o => o.ExternalId == registrationId);
            if (registration is null)
            {
                throw new KeyNotFoundException();
            }

            var task = await context
                .LookupApplicantRegistrationTasks
                .SingleOrDefaultAsync(t => t.Name == taskName && !t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);

            if (task is null)
            {
                throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
            }

            // Create a new entity if it doesn't exist
            taskStatus = new ApplicantRegistrationTaskStatus
            {
                ExternalId = Guid.NewGuid(),
                RegistrationId = registration.Id,
                Task = task,
                TaskStatus = statusEntity,
            };

            await context.RegistrationTaskStatus.AddAsync(taskStatus);
        }
        else
        {
            // Update the existing entity
            taskStatus.TaskStatus = statusEntity;

            context.RegistrationTaskStatus.Update(taskStatus);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", taskName, registrationId, status);
    }

    public async Task UpdateApplicantRegistrationTaskStatusAsync(string taskName, Guid registrationId, TaskStatuses status)
    {
        logger.LogInformation("Updating applicant status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", taskName, registrationId, status);

        var statusEntity = await context.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

        var taskStatus = await GetTaskStatusAsync(taskName, registrationId);
        if (taskStatus is null)
        {
            var registration = await context.Registrations.FirstOrDefaultAsync(o => o.ExternalId == registrationId);
            if (registration is null)
            {
                throw new KeyNotFoundException();
            }

            var task = await context
                .LookupApplicantRegistrationTasks
                .SingleOrDefaultAsync(t => t.Name == taskName && t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);

            if (task is null)
            {
                throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
            }

            // Create a new entity if it doesn't exist
            taskStatus = new ApplicantRegistrationTaskStatus
            {
                ExternalId = Guid.NewGuid(),
                RegistrationId = registration.Id,
                Task = task,
                TaskStatus = statusEntity,
            };

            await context.RegistrationTaskStatus.AddAsync(taskStatus);
        }
        else
        {
            // Update the existing entity
            taskStatus.TaskStatus = statusEntity;

            context.RegistrationTaskStatus.Update(taskStatus);
        }

        await context.SaveChangesAsync();

        logger.LogInformation("Successfully updated status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", taskName, registrationId, status);
    }


    public async Task UpdateSiteAddressAsync(Guid registrationId, AddressDto reprocessingSiteAddress)
    {
        var registration = await context.Registrations.FirstOrDefaultAsync(x => x.ExternalId == registrationId);

        if (registration is null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        // Reprocessing Site Address
        if (reprocessingSiteAddress.Id.GetValueOrDefault() == 0)
        {
            var address = new Address
            {
                AddressLine1 = reprocessingSiteAddress.AddressLine1,
                AddressLine2 = reprocessingSiteAddress.AddressLine2,
                TownCity = reprocessingSiteAddress.TownCity,
                County = reprocessingSiteAddress.County,
                PostCode = reprocessingSiteAddress.PostCode,
                NationId = reprocessingSiteAddress.NationId,
                GridReference = reprocessingSiteAddress.GridReference
            };

            await context.LookupAddresses.AddAsync(address);
            await context.SaveChangesAsync();

            reprocessingSiteAddress.Id = address.Id;
        }

        registration.ReprocessingSiteAddressId = reprocessingSiteAddress.Id;
        await context.SaveChangesAsync();
    }

    private IIncludableQueryable<Registration,List<RegistrationMaterial>?> LoadRegistrationWithRelatedEntities()
    {
        var data = context.Registrations
            .AsSplitQuery()
            .Include(r => r.BusinessAddress)
            .Include(r => r.ReprocessingSiteAddress)
            .Include(r => r.LegalDocumentAddress)
             
            .Include(r => r.ApplicantRegistrationTasksStatus)!
            .ThenInclude( t=>t.Task)

            .Include(r => r.ApplicantRegistrationTasksStatus)!
            .ThenInclude(t => t.TaskStatus)
             
            .Include(r => r.Materials);

        return data;
    }

    public async Task<IEnumerable<RegistrationOverviewDto>> GetRegistrationsOverviewForOrgIdAsync(Guid organisationId)
    {
        var registrations = await context.Registrations
            .AsSplitQuery()
            .Where(r => r.OrganisationId == organisationId)
            .Include(r => r.Materials!) // Include related Materials (non-nullable)
                .ThenInclude(m => m.Material) // Include nested Material
            .Include(r => r.Materials!) // Include related Materials again (non-nullable)
                .ThenInclude(m => m.Accreditations) // Include nested Accreditations
            .Include(r => r.ReprocessingSiteAddress) // Include ReprocessingSiteAddress
            .ToListAsync();
        var result = registrations.SelectMany(
            r => (r.Materials ?? []).DefaultIfEmpty(),
            (r, m) => new RegistrationOverviewDto
            {
                RegistrationId = r.ExternalId,
                RegistrationMaterialId = m?.Id ?? 0,
                MaterialId = m?.Material?.Id ?? 0,
                Material = m?.Material?.MaterialName ?? string.Empty,
                MaterialCode = m?.Material?.MaterialCode ?? string.Empty,
                ApplicationTypeId = r.ApplicationTypeId,
                RegistrationStatus = r.RegistrationStatusId,
                AccreditationStatus = m?.Accreditations?.FirstOrDefault()?.AccreditationStatus?.Id ?? 0,
                AccreditationYear = m?.Accreditations?.FirstOrDefault()?.AccreditationYear,
                ReprocessingSiteId = r.ReprocessingSiteAddressId,
                ReprocessingSiteAddress = r.ReprocessingSiteAddress != null ? new AddressDto
                {
                    Id = r.ReprocessingSiteAddress.Id,
                    AddressLine1 = r.ReprocessingSiteAddress.AddressLine1,
                    AddressLine2 = r.ReprocessingSiteAddress.AddressLine2,
                    TownCity = r.ReprocessingSiteAddress.TownCity,
                    County = r.ReprocessingSiteAddress.County,
                    PostCode = r.ReprocessingSiteAddress.PostCode,
                    NationId = r.ReprocessingSiteAddress.NationId ?? 0,
                    GridReference = r.ReprocessingSiteAddress.GridReference
                } : null,
                RegistrationYear = null // not required for MLS
            });

        return result;
    }
}