using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialsOutcomeCommand : IRequest
{
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }
    public RegistrationMaterialStatus RegistrationMaterialStatus { get; set; }
    public string? Comments { get; set; }   
}