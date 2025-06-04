using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpdateRegistrationSiteAddressHandler(IRegistrationRepository repository)
    : IRequestHandler<UpdateRegistrationSiteAddressCommand>
{
    public async Task Handle(UpdateRegistrationSiteAddressCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateSiteAddressAsync(command.RegistrationId, command.ReprocessingSiteAddress);
    }
}