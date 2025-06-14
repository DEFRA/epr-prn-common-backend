using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories;

public class RegistrationRepository(EprContext context, ILogger<RegistrationRepository> logger) : IRegistrationRepository
{
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

    public async Task<Registration?> GetAsync(int registrationId)
    {
        var registrations = LoadRegistrationWithRelatedEntities();
        return await registrations.SingleOrDefaultAsync(o => o.Id == registrationId);
    }

    public async Task<Registration?> GetByOrganisationAsync(int applicationTypeId, Guid organisationId)
    {
        var registrations = LoadRegistrationWithRelatedEntities();

        return await registrations
            .Where(o => o.ApplicationTypeId == applicationTypeId)
            .Where(o => o.OrganisationId == organisationId)
            .FirstOrDefaultAsync();
    }

    public async Task<ApplicantRegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationId)
    {
        var taskStatus = await context
            .RegistrationTaskStatus
            .Include(ts => ts.TaskStatus)
            .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.RegistrationId == registrationId);

        return taskStatus;
    }

    public async Task UpdateAsync(int registrationId, AddressDto businessAddress, AddressDto reprocessingSiteAddress,
        AddressDto legalDocumentsAddress)
    {
        var existing = await context.Registrations
            .Include(o => o.BusinessAddress)
            .Include(o => o.ReprocessingSiteAddress)
            .Include(o => o.LegalDocumentAddress)
            .FirstOrDefaultAsync(o => o.Id == registrationId);

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

    public async Task UpdateRegistrationTaskStatusAsync(string taskName, int registrationId, TaskStatuses status)
    {
        logger.LogInformation("Updating status for task with TaskName {TaskName} And RegistrationId {RegistrationId} to {Status}", taskName, registrationId, status);

        var statusEntity = await context.LookupTaskStatuses.SingleAsync(lts => lts.Name == status.ToString());

        var taskStatus = await GetTaskStatusAsync(taskName, registrationId);
        if (taskStatus is null)
        {
            var registration = await context.Registrations.FindAsync(registrationId);
            if (registration is null)
            {
                throw new KeyNotFoundException();
            }

            var task = await context
                .LookupTasks
                .SingleOrDefaultAsync(t => t.Name == taskName && !t.IsMaterialSpecific && t.ApplicationTypeId == registration.ApplicationTypeId);

            if (task is null)
            {
                throw new RegulatorInvalidOperationException($"No Valid Task Exists: {taskName}");
            }

            // Create a new entity if it doesn't exist
            taskStatus = new ApplicantRegistrationTaskStatus
            {
                ExternalId = Guid.NewGuid(),
                RegistrationId = registrationId,
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


    public async Task UpdateSiteAddressAsync(int registrationId, AddressDto reprocessingSiteAddress)
    {
        var registration = await context.Registrations.FirstOrDefaultAsync(x => x.Id == registrationId);

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
        return context.Registrations
            .AsSplitQuery()
            .Include(r => r.BusinessAddress)
            .Include(r => r.ReprocessingSiteAddress)
            .Include(r => r.LegalDocumentAddress)
            .Include(r => r.ApplicantRegistrationTasksStatus)!
                .ThenInclude(t => t.TaskStatus)
            .Include(r => r.ApplicantRegistrationTasksStatus)!
                .ThenInclude(t => t.Task)
            .Include(r => r.Materials);
    }
}