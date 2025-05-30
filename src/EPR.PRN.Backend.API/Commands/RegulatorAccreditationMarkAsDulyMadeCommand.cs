using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegulatorAccreditationMarkAsDulyMadeCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public DateTime DulyMadeDate { get; set; }
    public DateTime DeterminationDate { get; set; }
    public Guid DulyMadeBy { get; set; } 
}
