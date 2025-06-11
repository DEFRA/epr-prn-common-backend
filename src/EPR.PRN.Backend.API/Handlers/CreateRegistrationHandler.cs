using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class CreateRegistrationHandler(IRegistrationRepository repository)
    : IRequestHandler<CreateRegistrationCommand, int>
{
    public async Task<int> Handle(CreateRegistrationCommand command, CancellationToken cancellationToken)
    {
        var registrationId = await repository.CreateRegistrationAsync(command.ApplicationTypeId, command.OrganisationId, command.ReprocessingSiteAddress);

        await repository.UpdateRegistrationTaskStatusAsync(nameof(RegistrationTaskType.SiteAddressAndContactDetails),
            registrationId, TaskStatuses.Started);

        return registrationId;
    }
}