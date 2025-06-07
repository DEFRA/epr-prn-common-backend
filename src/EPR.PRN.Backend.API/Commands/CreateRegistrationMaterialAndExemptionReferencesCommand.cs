using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Obligation.Models;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class CreateRegistrationMaterialAndExemptionReferencesCommand: IRequest
{
    public required RegistrationMaterialDto RegistrationMaterial { get; set; }

    public required List<MaterialExemptionReferenceRequest> MaterialExemptionReferences { get; set; }
}
