using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetRegistrationOverviewDetailByIdHandler(
    IRegistrationMaterialRepository repo,
    IMapper mapper
) : IRequestHandler<GetRegistrationOverviewDetailByIdQuery, RegistrationOverviewDto>
{
    public async Task<RegistrationOverviewDto> Handle(GetRegistrationOverviewDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetRegistrationById(request.Id);
        var registrationDto = mapper.Map<RegistrationOverviewDto>(registration);

        var missingRegistrationTasks = await GetMissingTasks(registration.ApplicationTypeId, false, registrationDto.Tasks, registration.ReprocessingSiteAddress, registration.LegalDocumentAddress, registration.Materials);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);
        
        foreach (var materialDto in registrationDto.Materials)
        {
            var missingMaterialTasks = await GetMissingTasks(registration.ApplicationTypeId, true, materialDto.Tasks,null, null, null);
            materialDto.Tasks.AddRange(missingMaterialTasks);
        }

        return registrationDto;
    }
    
    private async Task<IEnumerable<RegistrationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, List<RegistrationTaskDto> existingTasks, LookupAddress? reprocessingSiteAddress, LookupAddress? LegalDocumentAddress, List<RegistrationMaterial>? materials)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific);
        var missingTasks = requiredTasks
            .Where(rt => existingTasks.All(r => r.TaskName != rt.Name))
            .Select(t =>
            {
                var taskDto = new RegistrationTaskDto
                {
                    TaskName = t.Name,
                    Status = RegulatorTaskStatus.NotStarted.ToString()
                };

                // Add taskData according to the task
                if (t.Name == "SiteAddressAndContactDetails" && reprocessingSiteAddress != null)
                {
                    taskDto.TaskData = new SiteAddressAndContactDetailsTaskDataDto
                    {
                        NationId = reprocessingSiteAddress.NationId,
                        SiteAddress = CreateAddressString(reprocessingSiteAddress),
                        GridReference = reprocessingSiteAddress.GridReference ?? "",
                        LegalCorrespondenceAddress = LegalDocumentAddress == null ? string.Empty : CreateAddressString(LegalDocumentAddress)
                    };
                }
                else if (t.Name == "MaterialsAuthorisedOnSite" && materials != null)
                {
                    var materialsList = new List<MaterialsAuthorisedOnSiteInfoDto>();

                    foreach (var material in materials)
                    {
                        materialsList.Add(new MaterialsAuthorisedOnSiteInfoDto
                        {
                            Material = material.Material.MaterialName,
                            RegistrationStatus = RegulatorTaskStatus.NotStarted.ToString(),
                            Reason = material.ReasonforNotreg
                        });
                    }

                    taskDto.TaskData = new MaterialsAuthorisedOnSiteTaskDataDto
                    {
                        RegistrationNumber = materials.FirstOrDefault().Wastecarrierbrokerdealerregistration, // Example hardcoded, you might change it
                        MaterialsAuthorisation = materialsList
                    };
                }
                

                return taskDto;
            });

        return missingTasks;
    }
    private static string CreateAddressString(LookupAddress address) =>
       string.Join(", ", new[]
       {
            address.AddressLine1,
            address.AddressLine2,
            address.TownCity,
            address.County,
            address.PostCode
       }.Where(x => !string.IsNullOrWhiteSpace(x)));
}

