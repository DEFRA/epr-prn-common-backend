using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// The handler that will execute when the command <see cref="UpdateMaximumWeightCommand"/> is sent.
/// </summary>
/// <param name="repository">Provides a means to manage registration materials.</param>
public class UpdateMaximumWeightCommandHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<UpdateMaximumWeightCommand>
{
    public async Task Handle(UpdateMaximumWeightCommand command, CancellationToken cancellationToken)
    {
        await repository.UpdateMaximumWeightForSiteAsync(command.RegistrationMaterialId, command.WeightInTonnes, command.PeriodId);
    }
}