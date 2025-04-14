namespace EPR.PRN.Backend.Data.Repositories;

using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
    private const string NotStarted = "NotStarted";
    protected readonly EprRegistrationsContext _eprContext = eprContext;
    public async Task UpdateRegistrationOutCome(int RegistrationMaterialId, int StatusId, string? Comment,String RegistrationReferenceNumber)
    {
        var material = await _eprContext.RegistrationMaterials.FirstOrDefaultAsync(rm => rm.Id == RegistrationMaterialId);

        if (material != null)
        {
            material.StatusID = StatusId;
            material.Comments = Comment;
            material.ReferenceNumber = RegistrationReferenceNumber;

            // No need to call Update explicitly if the entity is already tracked
            await _eprContext.SaveChangesAsync();
        }
    }
    public async Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId)
    {
        await Task.Delay(50);

        var registration = await _eprContext.Registrations.Where(r => r.Id == RegistrationId)
            .Select(r => new RegistrationOverviewDto
            {
                Id = r.Id,
                OrganisationName = r.OrganisationId + "_Green Ltd", // Replace with actual organisation name if needed
                OrganisationType = (ApplicationOrganisationType)r.ApplicationTypeId,
                Regulator = "EA",
                Tasks = GetRegistrationTasks(r),
                Materials = _eprContext.RegistrationMaterials
            .Where(rm => rm.RegistrationId == RegistrationId)
            .Select(rm => new RegistrationMaterialDto
            {
                Id = rm.Id,
                MaterialName = rm.Material.MaterialName,
                Status = rm.Status.Name,
                DeterminationDate = rm.DeterminationDate,
                RegistrationReferenceNumber = rm.ReferenceNumber,
                Tasks = GetRegistrationMaterialTasks(rm)
            })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (registration == null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        return registration;
    }

    private List<RegistrationTaskDto> GetRegistrationTasks(Registration registration)
    {
        var requiredRegistrationTaskDto = _eprContext.LookupTasks
            .Where(t => t.ApplicationTypeId == registration.ApplicationTypeId
            && t.IsMaterialSpecific == false
            && t.JourneyTypeId == 1).Select(t => new RegistrationTaskDto
            {
                Status = NotStarted,
                TaskName = t.Name,
                Id = null
            })
            .ToList();

        var existingRegistrationTaskDto = _eprContext.RegulatorRegistrationTaskStatus
            .Where(ts => ts.RegistrationId == registration.Id)
            .Select(t => new RegistrationTaskDto
            {
                Id = t.Id,
                TaskName = t.Task.Name,
                Status = t.TaskStatus.Name
            })
            .ToList();

        var mergedTasks = requiredRegistrationTaskDto
            .Select(requiredTask =>
                existingRegistrationTaskDto.FirstOrDefault(existingTask => existingTask.TaskName == requiredTask.TaskName)
                ?? requiredTask) // Use the existing task if found, otherwise keep the required task
            .ToList();

        return mergedTasks;
    }

    private List<RegistrationTaskDto> GetRegistrationMaterialTasks(RegistrationMaterial registrationMaterial)
    {
        var requiredRegistrationTaskDto = _eprContext.LookupTasks
            .Where(t => t.ApplicationTypeId == registrationMaterial.Registration.ApplicationTypeId
            && t.IsMaterialSpecific == true
            && t.JourneyTypeId == 1).Select(t => new RegistrationTaskDto
            {
                Status = NotStarted,
                TaskName = t.Name,
                Id = null
            })
            .ToList();

        var existingRegistrationTaskDto = _eprContext.RegulatorApplicationTaskStatus
            .Where(ts => ts.RegistrationMaterialId == registrationMaterial.Id)
            .Select(t => new RegistrationTaskDto
            {
                Id = t.Id,
                TaskName = t.Task.Name,
                Status = t.TaskStatus.Name
            })
            .ToList();

        var mergedTasks = requiredRegistrationTaskDto
            .Select(requiredTask =>
                existingRegistrationTaskDto.FirstOrDefault(existingTask => existingTask.TaskName == requiredTask.TaskName)
                ?? requiredTask) // Use the existing task if found, otherwise keep the required task
            .ToList();

        return mergedTasks;
    }

    public async Task<RegistrationMaterialDetailsDto> GetRegistrationMaterialDetailsById(int RegistrationMetrialId)
    {
        await Task.Delay(50);
        var result = await _eprContext.RegistrationMaterials
            .Where(rm => rm.Id == RegistrationMetrialId)
            .Select(rm => new RegistrationMaterialDetailsDto
            {
                Id = rm.Id,
                RegistrationId = rm.RegistrationId,
                MaterialName = _eprContext.LookupMaterials
                                .Where(m => m.Id == rm.MaterialId)
                                .Select(m => m.MaterialName)
                                .FirstOrDefault() ?? string.Empty,
                Status = (RegistrationMaterialStatus)_eprContext.LookupRegistrationMaterialStatuses
                    .Where(s => s.Id == rm.StatusID)
                    .Select(s => s.Id)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (result == null)
        {
            throw new KeyNotFoundException("Registration Material not found.");
        }

        return result;
    }

    public async Task<RegistrationReferenceBackendDto> GetRegistrationReferenceDataId(int RegistrationId,int RegistrationMetrialId)
    {
        var result = await _eprContext.Registrations
            .Where(r => r.Id == RegistrationId)
            .Select(r => new
            {
                r.ApplicationTypeId,
                r.BusinessAddressId,
                r.ReprocessingSiteAddressId,
                MaterialId = _eprContext.RegistrationMaterials
                    .Where(rm => rm.RegistrationId == r.Id && rm.Id == RegistrationMetrialId)
                    .Select(rm => rm.MaterialId)
                    .FirstOrDefault()
            })
            .ToListAsync(); // Change to ToListAsync to get a list

        var registrationReference = result
            .Select(r =>
            {
                var orgTypeEnum = (ApplicationOrganisationType)r.ApplicationTypeId;
                var organisationType = orgTypeEnum.ToString().First().ToString();

                var addressId = orgTypeEnum == ApplicationOrganisationType.Exporter
                    ? r.BusinessAddressId
                    : r.ReprocessingSiteAddressId;

                var country = _eprContext.LookupAddresses
                    .FirstOrDefault(ad => ad.Id == addressId)?.Country ?? "UNK";

                var countryCode = new string(country
                    .Take(3)
                    .ToArray())
                    .ToUpper();

                var materialCode = _eprContext.LookupMaterials
                    .FirstOrDefault(m => m.Id == r.MaterialId)?.MaterialCode ?? "UNKNOWN";

                return new RegistrationReferenceBackendDto
                {
                    OrganisationType = organisationType,
                    CountryCode = countryCode,
                    MaterialCode = materialCode
                };
            })
            .FirstOrDefault(); // Use FirstOrDefault on the list

        if (registrationReference == null)
        {
            throw new KeyNotFoundException("Registration Reference not found.");
        }

        return registrationReference;
    }
}