using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Registrationreprocessingio;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Handlers;

[ExcludeFromCodeCoverage]
public class CreateReprocessorOutputHandler(IRegistrationReprocessorIORepository repository)
    : IRequestHandler<CreateReprocessorOutputCommand>
{
    public async Task Handle(CreateReprocessorOutputCommand command, CancellationToken cancellationToken)
    {
        List<KeyValuePair<string, decimal>> MaterialTonnes = new List<KeyValuePair<string, decimal>>();
        foreach (var material in command.RawMaterialorProducts)
        {
            MaterialTonnes.Add(new KeyValuePair<string, decimal>(material.RawMaterialNameorProductName, material.TonneValue));
        }
        await repository.CreateReprocessorOutputAsync(command.ReprocessorOutputId, command.RegistrationMaterialId, command.SentToOtherSiteTonnes,
        command.ContaminantTonnes, command.ProcessLossTonnes, command.TotalOutputTonnes, MaterialTonnes);
        
    }
}