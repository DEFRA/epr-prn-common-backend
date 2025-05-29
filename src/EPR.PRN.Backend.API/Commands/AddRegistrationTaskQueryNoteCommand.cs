using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;
[ExcludeFromCodeCoverage]
public class AddRegistrationTaskQueryNoteCommand : IRequest
{
    [Required]
    [MaxLength(500)]
    public required string Note { get; set; }
    [BindNever]
    [SwaggerIgnore]
    public Guid RegulatorRegistrationTaskStatusId { get; set; }
    public Guid QueryBy { get; set; }
}