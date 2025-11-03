using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class CreateExemptionReferencesHandler(IRegistrationMaterialRepository repository)
    : IRequestHandler<CreateExemptionReferencesCommand>
{
    public async Task Handle(CreateExemptionReferencesCommand command, CancellationToken cancellationToken)
    {
        var exemptionReferences = command.MaterialExemptionReferences?.Select(x => new MaterialExemptionReference
        {
            ExternalId = Guid.NewGuid(),
            ReferenceNo = x.ReferenceNumber
        }).ToList() ?? new List<MaterialExemptionReference>();

        await repository.CreateExemptionReferencesAsync(command.RegistrationMaterialId, exemptionReferences);
    }
}
