using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

public class UpdateRegulatorRegistrationTaskCommand: IRequest<bool>
{
    [Required]
    [BindNever]
    [SwaggerIgnore]
    public int Id { get; set; }
    [Required]
    public required StatusTypes Status { get; set; }
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;
}

