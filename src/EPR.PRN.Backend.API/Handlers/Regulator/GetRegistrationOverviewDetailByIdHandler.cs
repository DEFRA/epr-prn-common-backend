using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetRegistrationOverviewDetailByIdHandler(
    IRegistrationRepository repo,
    IMapper mapper
) : IRequestHandler<GetRegistrationByIdQuery, RegistrationOverviewDto>
{
    public async Task<RegistrationOverviewDto> Handle(GetRegistrationByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetAsync(request.Id) ;
        var registrationDto = mapper.Map<RegistrationOverviewDto>(registration);

        var missingRegistrationTasks = await GetMissingTasks(registration.ApplicationTypeId, false, registrationDto.Tasks);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);

        foreach (var materialDto in registrationDto.Materials)
        {

            var missingMaterialTasks = await GetMissingTasks(registration.ApplicationTypeId, true, materialDto.Tasks);
            materialDto.Tasks.AddRange(missingMaterialTasks);
        }

        return registrationDto;
    }

    private async Task<IEnumerable<RegistrationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, List<RegistrationTaskDto> existingTasks)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific, 1);

        var missingTasks = requiredTasks
            .Where(rt => existingTasks.All(r => r.TaskName != rt.Name))
            .Select(t => new RegistrationTaskDto { TaskName = t.Name, Status = RegulatorTaskStatus.NotStarted.ToString() });

        return missingTasks;
    }
}