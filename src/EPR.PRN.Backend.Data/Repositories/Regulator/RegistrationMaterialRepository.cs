namespace EPR.PRN.Backend.Data.Repositories;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
using EPR.PRN.Backend.Data;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Microsoft.EntityFrameworkCore;

public class RegistrationMaterialRepository(EprRegistrationsContext eprContext) : IRegistrationMaterialRepository
{
    protected readonly EprRegistrationsContext _eprContext = eprContext;
    public async Task<bool> UpdateRegistrationOutCome(int RegistrationMaterialId, int Outcome, string? OutComeComment)
    {
        await Task.Delay(50);
        bool result = true;
        return result;
    }
    public async Task<RegistrationOverviewDto> GetRegistrationOverviewDetailById(int RegistrationId)
    {
        await Task.Delay(50);






        return new RegistrationOverviewDto
        {
            Id = RegistrationId,
            OrganisationName = "Green Ltd",
            OrganisationType = "Exporter",
            Regulator = "EA",
            Tasks = new()
                {
                    new RegistrationTaskDto {
                        Id =12,      // RegulatorRegistrationTaskStatus.Id                       
                        TaskId=1,   // Task.Id (lookup)                             
                        TaskName = "Business address",
                        Status = "Complete" },

                 },
            Materials = new()
                {
                    new RegistrationMaterialDto
                    {
                        Id = 101,
                        MaterialName = "Plastic",
                        Status = "Granted",
                        DeterminationDate = DateTime.UtcNow,
                        ReferenceNumber = "ABC123",
                         Tasks = new()
                        {
                            new RegistrationTaskDto {
                                Id = 45, // RegulatorRegistrationTaskStatus.Id
                                TaskId= 5,  // Task.Id (lookup)
                                TaskName = "Sampling plan",
                                Status = "Approved" },
                            new RegistrationTaskDto {
                                 Id = 46, // RegulatorRegistrationTaskStatus.Id
                                 TaskId= 6,  // Task.Id (lookup)                                
                                 TaskName = "Sampling and inspection plan",
                                 Status = "Approved" }
                        }
                    },
                    new RegistrationMaterialDto
                    {
                        Id = 102,
                        MaterialName = "Steel",
                        Status = "Refused",
                        DeterminationDate = DateTime.UtcNow,
                        ReferenceNumber = "DEF456",
                        Comments="Test description for Steel",
                        Tasks = new()
                        {
                            new RegistrationTaskDto {
                                Id = 47, // RegulatorRegistrationTaskStatus.Id
                                TaskId= 7,  // Task.Id (lookup)                                
                                TaskName = "Waste licences, permits or exemptions",
                                Status = "Not Started"
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
                         .FirstOrDefault(),
           Status = _eprContext.LookupTaskStatuses
                         .Where(s => s.Id == rm.StatusID)
                         .Select(s => s.Name)
                         .FirstOrDefault(),
           DeterminationDate = rm.DeterminationDate,
           ReferenceNumber = rm.ReferenceNumber,
           Tasks = _eprContext.RegulatorApplicationTaskStatus
               .Where(rt => rt.RegistrationMaterialId == rm.Id)
               .Select(rt => new RegistrationTaskDto
               {
                   Id = rt.Id,
                   TaskId = rt.TaskId,
                   TaskName = _eprContext.LookupTasks
                              .Where(t => t.Id == rt.TaskId)
                   .Select(t => t.Name)
                              .FirstOrDefault(),
                   Status = _eprContext.LookupTaskStatuses
                             .Where(ts => ts.Id == rt.Id)
                             .Select(ts => ts.Name)
                             .FirstOrDefault()
               }).ToList()
       })
       .FirstOrDefaultAsync();

        return result;
        //return new RegistrationMaterialDto
        //{
        //    Id = RegistrationMetrialId,
        //    MaterialName = "Plastic",
        //    Status = "Granted",
        //    DeterminationDate = DateTime.UtcNow,
        //    ReferenceNumber = "ABC123",
        //    Tasks = new()
        //    {
        //        new RegistrationTaskDto {
        //            Id = 45, // RegulatorRegistrationTaskStatus.Id
        //            TaskId= 5,  // Task.Id (lookup)
        //            TaskName = "Sampling plan",
        //            Status = "Approved"
        //        }
        //    }
        //};

    }
}


