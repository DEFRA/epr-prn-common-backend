using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
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

        var missingRegistrationTasks = await GetMissingTasks(registration.ApplicationTypeId, false, registrationDto.Tasks);
        registrationDto.Tasks.AddRange(missingRegistrationTasks);

        registrationDto.Materials = registrationDto.Materials.Where(m => m.IsMaterialRegistered).ToList();
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