using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationHandler(IRegistrationRepository repository, IMapper mapper)
    : IRequestHandler<CreateRegistrationCommand, CreateRegistrationDto>
{
    public async Task<CreateRegistrationDto> Handle(CreateRegistrationCommand command, CancellationToken cancellationToken)
    {
        var registration = await repository.CreateRegistrationAsync(command.ApplicationTypeId, command.OrganisationId, command.ReprocessingSiteAddress);

        await repository.UpdateRegistrationTaskStatusAsync(nameof(RegistrationTaskType.SiteAddressAndContactDetails),
            registration.ExternalId, TaskStatuses.Started);

        return mapper.Map<CreateRegistrationDto>(registration);
    }
}