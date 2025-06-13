using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class CreateExemptionReferencesCommand : IRequest
{
    public int RegistrationMaterialId { get; set; }
    
    public required List<MaterialExemptionReferenceDto> MaterialExemptionReferences { get; set; }
}
