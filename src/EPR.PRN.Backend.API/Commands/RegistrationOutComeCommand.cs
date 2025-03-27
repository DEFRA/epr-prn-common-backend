#nullable disable
using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Commands;

[ExcludeFromCodeCoverage]
public class RegistrationOutComeCommand: IRequest<bool>
{
    [Required]
    public Guid RegistrationID { get; set; }
    [Required]
    public Guid MaterialID { get; set; }
    public required OutComeTypes OutCome { get; set; }
    [MaxLength(1000)]
    public string? OutComeComment { get; set; } = string.Empty;
   
}