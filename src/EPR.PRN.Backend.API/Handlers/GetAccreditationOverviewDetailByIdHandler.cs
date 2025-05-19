using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class GetAccreditationOverviewDetailByIdHandler(
    IRegistrationMaterialRepository repo,
    IMapper mapper
) : IRequestHandler<GetAccreditationOverviewDetailByIdQuery, RegistrationOverviewDto>
{
    public async Task<RegistrationOverviewDto> Handle(GetAccreditationOverviewDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetRegistrationByExternalIdAndYear(request.Id, request.Year);
        var registrationDto = mapper.Map<RegistrationOverviewDto>(registration);

        var missingRegistrationTasks = await GetMissingTasks(registration.ApplicationTypeId, false, 2, registrationDto.AccreditationTasks, request.Year);
        registrationDto.AccreditationTasks.AddRange(missingRegistrationTasks);

        registrationDto.Materials = registrationDto.Materials.Where(m => m.IsMaterialRegistered).ToList();
        foreach (var materialDto in registrationDto.Materials)
        {
            foreach (var accreditationDto in materialDto.Accreditations)
            {
                var missingMaterialTasks = await GetMissingTasks(registration.ApplicationTypeId, true, 2, accreditationDto.Tasks);
                accreditationDto.Tasks.AddRange(missingMaterialTasks);
            }
        }

        return registrationDto;
    }

    private async Task<IEnumerable<AccreditationRegistrationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId, List<AccreditationRegistrationTaskDto> existingTasks, int year)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific, journeyTypeId);

        var missingTasks = requiredTasks
            .Where(rt => existingTasks.All(r => r.TaskName != rt.Name))
            .Select(t => new AccreditationRegistrationTaskDto { TaskName = t.Name, Status = RegulatorTaskStatus.NotStarted.ToString(), AccreditationYear = year });

        return missingTasks;
    }
    private async Task<IEnumerable<AccreditationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId, List<AccreditationTaskDto> existingTasks)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific, journeyTypeId);

        var missingTasks = requiredTasks
            .Where(rt => existingTasks.All(r => r.TaskName != rt.Name))
            .Select(t => new AccreditationTaskDto { TaskName = t.Name, Status = RegulatorTaskStatus.NotStarted.ToString() });

        return missingTasks;
    }
}