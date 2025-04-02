namespace EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data;

public class RegistrationMaterialRepository(EprContext eprContext, ILogger<Repository> logger, IConfiguration configuration) : IRegistrationMaterialRepository
{
    protected readonly EprContext _eprContext = eprContext;
    private readonly string logPrefix = configuration["LogPrefix"];


    public async Task<bool> UpdateRegistrationOutCome(int RegistrationMaterialId, int Outcome, string? OutComeComment)
    {
        await Task.Delay(50);
        bool result = true;
        logger.LogInformation("Update Registration OutCome. RegistrationMaterialId : {RegistrationMaterialId} and {Outcome}", RegistrationMaterialId, Outcome);
        return true;
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
        return new RegistrationMaterialDto
        {
            Id = RegistrationMetrialId,
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
                    Status = "Approved"
                }
            }
        };

    }
}


