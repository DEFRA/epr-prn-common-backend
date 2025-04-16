using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public abstract class UpdateRegulatorTaskCommandBase : IRequest
{
    [Required]
    public required string TaskName { get; set; }
    [Required]
    public required RegulatorTaskStatus Status { get; set; }
    [MaxLength(500)]
    public string? Comments { get; set; } = string.Empty;

    public abstract int TypeId { get; }
    public required string UserName { get; set; }
}