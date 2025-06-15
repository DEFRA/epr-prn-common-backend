using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage(Justification = "TODO: To be done as part of create registration user story")]
public class UpdateRegistrationHandler(IRegistrationRepository repository)
    : IRequestHandler<UpdateRegistrationCommand>
{
    public async Task Handle(UpdateRegistrationCommand command, CancellationToken cancellationToken)
    {
        await repository
            .UpdateAsync(command.RegistrationId, command.BusinessAddress, command.ReprocessingSiteAddress, command.LegalAddress);
    }
}