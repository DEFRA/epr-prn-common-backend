using EPR.PRN.Backend.API.Common.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

public class RegistrationTaskStatusDto
{
    public required StatusTypes Status { get; set; }
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;
}

public class UpdateRegulatorRegistrationTaskCommand: RegistrationTaskStatusDto, IRequest<bool>
{
    [Required]
    internal int Id { get; set; }
}