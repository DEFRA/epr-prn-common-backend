namespace EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Extensions;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
    protected readonly EprRegistrationsContext _eprContext = eprContext;
    public async Task<string> UpdateRegistrationOutCome(int RegistrationMaterialId, int StatusId, string? Comment,String RegistrationReferenceNumber)
    {
        await Task.Delay(50);
        
        return RegistrationReferenceNumber;
    }
    public async Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId)
    {
        await Task.Delay(50);

        return new RegistrationOverviewDto
        {
            Id = RegistrationId,
            OrganisationName = "Green Ltd",
            OrganisationType = ApplicationOrganisationType.Exporter,
            Regulator = "EA",
            Tasks = new()
            {
                new RegistrationTaskDto {
                    Id = 12,      // RegulatorRegistrationTaskStatus.Id                       
                    TaskId = 1,   // Task.Id (lookup)                             
                    TaskName = RegulatorTaskType.BusinessAddress,
                    Status = RegulatorTaskStatus.NotStarted
                }
            },
            Materials = new()
            {
                new RegistrationMaterialDto
                {
                    Id = 101,
                    MaterialName = "Plastic",
                    Status = RegistrationMaterialStatus.GRANTED,
                    DeterminationDate = DateTime.UtcNow,
                    RegistrationReferenceNumber = "ABC123",
                    Tasks = new()
                    {
                        new RegistrationTaskDto {
                            Id = 45, // RegulatorRegistrationTaskStatus.Id
                            TaskId = 5,  // Task.Id (lookup)
                            TaskName = RegulatorTaskType.SamplingAndInspectionPlan,
                            Status = RegulatorTaskStatus.Approved
                        },
                        new RegistrationTaskDto {
                            Id = 46, // RegulatorRegistrationTaskStatus.Id
                            TaskId = 6,  // Task.Id (lookup)
                            TaskName = RegulatorTaskType.MaterialsAuthorisedOnSite,
                            Status = RegulatorTaskStatus.Approved
                        }
                    },
                },
                new RegistrationMaterialDto
                {
                    Id = 102,
                    MaterialName = "Steel",
                    Status = RegistrationMaterialStatus.REFUSED,
                    DeterminationDate = DateTime.UtcNow,
                    RegistrationReferenceNumber = "DEF456",
                    Comments = "Test description for Steel",
                    Tasks = new()
                    {
                        new RegistrationTaskDto {
                            Id = 47, // RegulatorRegistrationTaskStatus.Id
                            TaskId = 7,  // Task.Id (lookup)                                
                            TaskName = RegulatorTaskType.WasteLicensesPermitsAndExemptions,
                            Status = RegulatorTaskStatus.NotStarted
                        }
                    }
                }
            }
        };

    }
    public async Task<RegistrationMaterialDto> GetMaterialsById(int RegistrationMetrialId)
    {
        await Task.Delay(50);
        var result = await _eprContext.RegistrationMaterials
            .Where(rm => rm.Id == RegistrationMetrialId)
            .Select(rm => new RegistrationMaterialDto
            {
                Id = rm.Id,
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

        return result;
    }
}


