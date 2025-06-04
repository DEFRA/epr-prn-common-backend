using EPR.PRN.Backend.Obligation.Models;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class CreateMaterialExemptionReferenceCommand :IRequest<bool>
{
    public required List<MaterialExemptionReferenceRequest> MaterialExemptionReferences { get; set; }
}
