namespace EPR.PRN.Backend.Data.Repositories;

using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
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
                Tasks = _eprContext.RegulatorRegistrationTaskStatus
                .Where(t => t.RegistrationId == RegistrationId)
                .Select(t => new RegistrationTaskDto
                {
                    Id = t.Id,
                    TaskId = t.TaskId ?? 0,
                    TaskName = (RegulatorTaskType)_eprContext.LookupTasks
                        .Where(tn => tn.Id == t.TaskId)
                        .Select(tn => tn.Id)
                        .FirstOrDefault(),
                    Status = (RegulatorTaskStatus)_eprContext.LookupTaskStatuses
                        .Where(ts => ts.Id == t.TaskStatusId)
                        .Select(ts => ts.Id)
                        .FirstOrDefault()
                }).ToList(),          

                Materials = _eprContext.RegistrationMaterials
            .Where(rm => rm.RegistrationId == RegistrationId)
            .Select(rm => new RegistrationMaterialDto
            {
                Id = rm.Id,
                MaterialName = _eprContext.LookupMaterials
                                .Where(m => m.Id == rm.MaterialId)
                                .Select(m => m.MaterialName)
                                .FirstOrDefault() ?? string.Empty,
                Status = (RegistrationMaterialStatus)_eprContext.LookupTaskStatuses
                    .Where(s => s.Id == rm.StatusID)
                    .Select(s => s.Id)
                    .FirstOrDefault(),
                DeterminationDate = rm.DeterminationDate,
                RegistrationReferenceNumber = rm.ReferenceNumber,
                Tasks = _eprContext.RegulatorRegistrationTaskStatus
                    .Where(rt => rt.Id == rm.Id)
                    .Select(rt => new RegistrationTaskDto
                    {
                        Id = rt.Id,
                        TaskId = rt.TaskId ?? 0, // Provide a default value if TaskId is null
                        TaskName = (RegulatorTaskType)_eprContext.LookupTasks
                            .Where(t => t.Id == rt.TaskId)
                            .Select(t => t.Id)
                            .FirstOrDefault(),
                        Status = (RegulatorTaskStatus)_eprContext.LookupTaskStatuses
                            .Where(ts => ts.Id == rt.TaskStatusId)
                            .Select(ts => ts.Id)
                            .FirstOrDefault()
                    }).ToList()
            })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (registration == null)
        {
            throw new InvalidOperationException("Registration not found.");
        }

        return registration;
    

    }
    public async Task<RegistrationMaterialDto> GetMaterialsById(int RegistrationMetrialId)
    {
        await Task.Delay(50);
        var result = await _eprContext.RegistrationMaterials
            .Where(rm => rm.Id == RegistrationMetrialId)
            .Select(rm => new RegistrationMaterialDto
            {
                Id = rm.Id,
                RegistrationId = rm.RegistrationId,
                MaterialName = _eprContext.LookupMaterials
                                .Where(m => m.Id == rm.MaterialId)
                                .Select(m => m.MaterialName)
                                .FirstOrDefault()??string.Empty,
                Status = (RegistrationMaterialStatus)_eprContext.LookupTaskStatuses
                    .Where(s => s.Id == rm.StatusID)
                    .Select(s => s.Id)
                    .FirstOrDefault(),
                DeterminationDate = rm.DeterminationDate,
                RegistrationReferenceNumber = rm.ReferenceNumber,
                Tasks = _eprContext.RegulatorRegistrationTaskStatus
                    .Where(rt => rt.Id == rm.Id)
                    .Select(rt => new RegistrationTaskDto
                    {
                        Id = rt.Id,
                        TaskId = rt.TaskId ?? 0, // Provide a default value if TaskId is null
                        TaskName = (RegulatorTaskType)_eprContext.LookupTasks
                            .Where(t => t.Id == rt.TaskId.Value)
                            .Select(t => t.Id)
                            .FirstOrDefault(),
                        Status = (RegulatorTaskStatus)_eprContext.LookupTaskStatuses
                            .Where(ts => ts.Id == rt.Id)
                            .Select(ts => ts.Id)
                            .FirstOrDefault()
                    }).ToList()
            })
            .FirstOrDefaultAsync();


        if (result == null)
        {
            throw new InvalidOperationException("Registration Material not found.");
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
            throw new InvalidOperationException("Registration Reference not found.");
        }

        return registrationReference;
    }
}