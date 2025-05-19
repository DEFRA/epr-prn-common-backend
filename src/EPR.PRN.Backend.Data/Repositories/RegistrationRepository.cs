using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.PRN.Backend.Data.Repositories;

public class RegistrationRepository(EprContext context, ILogger<RegistrationRepository> logger) : IRegistrationRepository
{
    public async Task<RegistrationTaskStatus?> GetTaskStatusAsync(string taskName, int registrationId)
    {
        var taskStatus = await context
            .RegistrationTaskStatus
            .Include(ts => ts.TaskStatus)
            .FirstOrDefaultAsync(x => x.Task.Name == taskName && x.RegistrationId == registrationId);

        return taskStatus;
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
            taskStatus = new RegistrationTaskStatus
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


    public async Task UpdateSiteAddressAsync(int registrationId, AddressDto reprocessingSiteAddress, AddressDto legalDocumentAddress)
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

            reprocessingSiteAddress.Id = address.Id;
        }

        // Legal Document Address
        if (legalDocumentAddress.Id.GetValueOrDefault() == 0)
        {
            var address = new Address
            {
                AddressLine1 = legalDocumentAddress.AddressLine1,
                AddressLine2 = legalDocumentAddress.AddressLine2,
                TownCity = legalDocumentAddress.TownCity,
                County = legalDocumentAddress.County,
                PostCode = legalDocumentAddress.PostCode,
                NationId = legalDocumentAddress.NationId,
                GridReference = legalDocumentAddress.GridReference
            };

            await context.LookupAddresses.AddAsync(address);

            legalDocumentAddress.Id = address.Id;
        }

        registration.ReprocessingSiteAddressId = reprocessingSiteAddress.Id;
        registration.LegalDocumentAddressId = legalDocumentAddress.Id;

        await context.SaveChangesAsync();
    }
}
