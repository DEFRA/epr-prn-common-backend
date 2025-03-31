using EPR.PRN.Backend.API.Common.Enums;
using System.ComponentModel.DataAnnotations;

public class UpdateTaskStatusRequestDto
{
    public required StatusTypes Status { get; set; }
    [MaxLength(200)]
    public string? Comment { get; set; } = string.Empty;
}

