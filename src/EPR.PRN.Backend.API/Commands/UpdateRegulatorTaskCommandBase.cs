using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public abstract class UpdateRegulatorTaskCommandBase : IRequest
{
    [Required]
    public required string TaskName { get; set; }
    [Required]
    public required StatusTypes Status { get; set; }
    [MaxLength(500)]
    public string? Comment { get; set; } = string.Empty;

    public abstract int TypeId { get; }
    public required string UserName { get; set; }
}