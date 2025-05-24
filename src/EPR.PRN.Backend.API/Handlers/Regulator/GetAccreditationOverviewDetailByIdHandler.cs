using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetAccreditationOverviewDetailByIdHandler(
    IRegistrationMaterialRepository repo,
    IMapper mapper
) : IRequestHandler<GetAccreditationOverviewDetailByIdQuery, RegistrationOverviewDto>
{
    public async Task<RegistrationOverviewDto> Handle(GetAccreditationOverviewDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var registration = await repo.GetRegistrationByExternalIdAndYear(request.Id, request.Year);
        var registrationDto = mapper.Map<RegistrationOverviewDto>(registration);

        List<int> accreditationYears = new List<int>{ };

        foreach (var materialDto in registrationDto.Materials)
        {
            foreach (var accreditationDto in materialDto.Accreditations)
            {
                if (!accreditationYears.Contains(accreditationDto.AccreditationYear))
                {
                    accreditationYears.Add(accreditationDto.AccreditationYear);
                }

                var missingMaterialTasks = await GetMissingTasks(registration.ApplicationTypeId, true, 2, accreditationDto.Tasks, accreditationDto.AccreditationYear);
                accreditationDto.Tasks.AddRange(missingMaterialTasks);
            }
        }

        foreach (int year in accreditationYears)
        {
            var missingRegistrationTasks = await GetMissingTasks(registration.ApplicationTypeId, false, 2, registrationDto.Tasks, year);
            registrationDto.Tasks.AddRange(missingRegistrationTasks);
        }

        return registrationDto;
    }

    private async Task<IEnumerable<RegistrationTaskDto>> GetMissingTasks(int applicationTypeId, bool isMaterialSpecific, int journeyTypeId, List<RegistrationTaskDto> existingTasks, int? year)
    {
        var requiredTasks = await repo.GetRequiredTasks(applicationTypeId, isMaterialSpecific, journeyTypeId);

        var missingTasks = requiredTasks
            .Where(rt => !existingTasks.Any(r => r.TaskName == rt.Name && r.Year == year))
            .Select(t => new RegistrationTaskDto { TaskName = t.Name, Status = RegulatorTaskStatus.NotStarted.ToString(), Year = year });

        return missingTasks;
    }
}