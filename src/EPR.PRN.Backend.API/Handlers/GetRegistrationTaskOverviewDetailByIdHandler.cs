using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetRegistrationTaskOverviewDetailByIdHandler(
    IRegistrationRepository repo,
    IMapper mapper
) : IRequestHandler<GetRegistrationTaskOverviewByIdQuery, ApplicantRegistrationTasksOverviewDto>
{
    public async Task<ApplicantRegistrationTasksOverviewDto> Handle(GetRegistrationTaskOverviewByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetTasksForRegistrationAndMaterialsAsync(request.Id);
        var registrationDto = mapper.Map<ApplicantRegistrationTasksOverviewDto>(registration);

        var applicationTypeId = registration.ApplicationTypeId;
        var journeyTypeId = (int)JourneyType.Registration;
        var requiredRegistrationTasks = await repo.GetRequiredTasks(applicationTypeId, false, journeyTypeId);
        var requiredRegistrationMaterialTasks = await repo.GetRequiredTasks(applicationTypeId, true, journeyTypeId);

        var missingRegistrationTasks = GetMissingTasks(requiredRegistrationTasks, registrationDto.Tasks);
        var missingRegistrationMaterialTasks = GetMissingTasks(requiredRegistrationMaterialTasks, registrationDto.Tasks);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);
        registrationDto.Tasks.AddRange(missingRegistrationMaterialTasks);

        if (registrationDto.Materials.Count is 0)
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

    private static IEnumerable<ApplicantRegistrationTaskDto> GetMissingTasks(List<LookupApplicantRegistrationTask> requiredTasks, List<ApplicantRegistrationTaskDto> existingTasks)
    {
        var existingTaskNames = new HashSet<string>(existingTasks.Select(r => r.TaskName));

        // Bring back all required tasks for the application and journey type that are not already in the list.
        var missingTasks = requiredTasks
            .Where(rt => !existingTaskNames.Contains(rt.Name))
            .Select(rt => new ApplicantRegistrationTaskDto
            {
                TaskName = rt.Name,
                Status = nameof(RegulatorTaskStatus.CannotStartYet)
            });

        return missingTasks;
    }
}