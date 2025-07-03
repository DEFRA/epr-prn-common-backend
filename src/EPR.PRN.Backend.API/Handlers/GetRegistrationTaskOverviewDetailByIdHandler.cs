using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
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
        var journeyTypeId = (int)JourneyType.Registration;
        var requiredRegistrationTasks = await repo.GetRequiredTasks(applicationTypeId, false, journeyTypeId);
        var requiredRegistrationMaterialTasks = await repo.GetRequiredTasks(applicationTypeId, true, journeyTypeId);

        var missingRegistrationTasks = GetMissingTasks(requiredRegistrationTasks, registrationDto.Tasks);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);

        if (registrationDto.Materials.Count == 0)
        {
            return registrationDto;
        }

        Parallel.ForEach(registrationDto.Materials, (materialDto, _) =>
        {
            var missingMaterialTasks = GetMissingTasks(requiredRegistrationMaterialTasks, materialDto.Tasks);
            materialDto.Tasks.AddRange(missingMaterialTasks);
        });

        return registrationDto;
    }

    private IEnumerable<RegistrationTaskDto> GetMissingTasks(List<LookupApplicantRegistrationTask> requiredTasks, List<RegistrationTaskDto> existingTasks)
    {
        var existingTaskNames = new HashSet<string>(existingTasks.Select(r => r.TaskName));

        // Bring back all required tasks for the application and journey type that are not already in the list.
        var missingTasks = requiredTasks
            .Where(rt => !existingTaskNames.Contains(rt.Name))
            .Select(rt => new RegistrationTaskDto
            {
                TaskName = rt.Name,
                Status = nameof(RegulatorTaskStatus.NotStarted)
            });

        return missingTasks;
    }
}