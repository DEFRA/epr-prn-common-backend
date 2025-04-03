#nullable disable
using EPR.PRN.Backend.API.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialsOutcomeCommand : IRequest<HandlerResponse<bool>>
{
    [Required]
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }
    [Required]
    public required string Outcome { get; set; }
    [MaxLength(200)]
    public string? Comments { get; set; }   
}