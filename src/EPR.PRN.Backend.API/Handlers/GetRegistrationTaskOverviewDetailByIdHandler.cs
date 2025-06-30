using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetRegistrationTaskOverviewDetailByIdHandler(
    IRegistrationRepository repo,
    IMapper mapper
) : IRequestHandler<GetRegistrationTaskOverviewByIdQuery, RegistrationTaskOverviewDto>
{
    public async Task<RegistrationTaskOverviewDto> Handle(GetRegistrationTaskOverviewByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetAsync(request.Id);
        var registrationDto = mapper.Map<RegistrationTaskOverviewDto>(registration!);

        var applicationTypeId = (int)registration?.ApplicationTypeId!;
        
        var missingRegistrationTasks = await GetMissingTasks(applicationTypeId, false, registrationDto.Tasks);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);

        if (registrationDto.Materials.Count == 0)
        {
            return registrationDto;
        }

        await Parallel.ForEachAsync(registrationDto.Materials, cancellationToken, async (materialDto, _) =>
        {
            var missingMaterialTasks = await GetMissingTasks(registration.ApplicationTypeId, true, materialDto.Tasks);
            materialDto.Tasks.AddRange(missingMaterialTasks);
        });

        return registrationDto;
    }

    private async Task<IEnumerable<RegistrationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, List<RegistrationTaskDto> existingTasks)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific, 1);

        var missingTasks = requiredTasks
            .Where(rt => existingTasks.TrueForAll(r => r.TaskName != rt.Name))
            .Select(t => new RegistrationTaskDto { TaskName = t.Name, Status = nameof(RegulatorTaskStatus.NotStarted)});

        return missingTasks;
    }
}

