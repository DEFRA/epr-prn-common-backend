using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegulatorAccreditationMarkAsDulyMadeCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid Id { get; set; }
    public DateTime DulyMadeDate { get; set; }
    public DateTime DeterminationDate { get; set; }
    public Guid DulyMadeBy { get; set; }
}
