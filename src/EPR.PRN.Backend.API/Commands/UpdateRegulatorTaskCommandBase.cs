using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.API.Common.Enums;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;
public abstract class UpdateRegulatorTaskCommandBase : IRequest
{
    [Required]
    public required string TaskName { get; set; }
    [Required]
    public required RegulatorTaskStatus Status { get; set; }
    [MaxLength(500)]
    public string? Comments { get; set; } = string.Empty;
    public string? UserName { get; set; } = string.Empty;
}