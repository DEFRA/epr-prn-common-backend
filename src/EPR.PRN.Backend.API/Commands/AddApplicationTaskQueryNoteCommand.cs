using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.PRN.Backend.API.Commands;
[ExcludeFromCodeCoverage]
public class AddApplicationTaskQueryNoteCommand : IRequest
{
    [Required]
    public required string Note { get; set; }
    [BindNever]
    [SwaggerIgnore]
    public Guid RegulatorApplicationTaskStatusId { get; set; }
    public Guid CreatedBy { get; set; }
}