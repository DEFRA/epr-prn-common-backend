using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Obligation.Models;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class CreateRegistrationMaterialAndExemptionReferencesCommand: IRequest
{
    public RegistrationMaterialDto RegistrationMaterial { get; set; }

    public List<MaterialExemptionReferenceRequest> MaterialExemptionReferences { get; set; }
}
