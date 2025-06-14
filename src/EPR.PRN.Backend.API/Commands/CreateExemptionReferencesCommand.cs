using EPR.PRN.Backend.API.Dto.Regulator;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

public class CreateExemptionReferencesCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid RegistrationMaterialId { get; set; }
    
    public required List<MaterialExemptionReferenceDto> MaterialExemptionReferences { get; set; }
}
