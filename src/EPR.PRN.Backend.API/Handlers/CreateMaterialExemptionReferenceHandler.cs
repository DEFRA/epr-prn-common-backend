using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Services.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class CreateMaterialExemptionReferenceHandler(IMaterialExemptionReferenceService materialExemptionReferenceService) 
    : IRequestHandler<CreateMaterialExemptionReferenceCommand, bool>
{
    public async Task<bool> Handle(CreateMaterialExemptionReferenceCommand command, CancellationToken cancellationToken)
    {
        return await materialExemptionReferenceService.CreateMaterialExemptionReferenceAsync(command.MaterialExemptionReferences, cancellationToken);
    }
}
