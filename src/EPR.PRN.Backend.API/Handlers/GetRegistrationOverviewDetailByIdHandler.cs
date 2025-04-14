using AutoMapper;
using EPR.PRN.Backend.API.Common.Dto.Regulator;
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

        var requiredTasks = await repo.GetRequiredTasks(registration.ApplicationTypeId, false);
        var existingTasks = await repo.GetRegistrationTasks(request.Id);

        registrationDto.Tasks = requiredTasks
            .Select(rt => existingTasks.FirstOrDefault(et => et.Task.Name == rt.Name) is { } existing
                ? mapper.Map<RegistrationTaskDto>(existing)
                : new RegistrationTaskDto { TaskName = rt.Name, Status = "NotStarted" })
            .ToList();

        var materials = await repo.GetMaterialsByRegistrationId(request.Id);
        registrationDto.Materials = new List<RegistrationMaterialDto>();

        foreach (var material in materials)
        {
            var materialDto = mapper.Map<RegistrationMaterialDto>(material);

            var requiredMaterialTasks = await repo.GetRequiredTasks(material.Registration.ApplicationTypeId, true);
            var existingMaterialTasks = await repo.GetMaterialTasks(material.Id);

            materialDto.Tasks = requiredMaterialTasks
                .Select(rt => existingMaterialTasks.FirstOrDefault(et => et.Task.Name == rt.Name) is { } existing
                    ? mapper.Map<RegistrationTaskDto>(existing)
                    : new RegistrationTaskDto { TaskName = rt.Name, Status = "NotStarted" })
                .ToList();

            registrationDto.Materials.Add(materialDto);
        }

        return registrationDto;
    }
}