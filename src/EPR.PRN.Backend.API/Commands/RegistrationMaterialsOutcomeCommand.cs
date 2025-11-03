using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialsOutcomeCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public Guid Id { get; set; }
    public RegistrationMaterialStatus Status { get; set; }
    public string? Comments { get; set; }
    public required string RegistrationReferenceNumber { get; set; }
    public Guid User { get; set; }
}