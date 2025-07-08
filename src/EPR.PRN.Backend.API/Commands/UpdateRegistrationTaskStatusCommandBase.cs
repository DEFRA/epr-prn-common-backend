using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Commands;

public class UpdateRegistrationTaskStatusCommandBase
{
    public string TaskName { get; set; } = string.Empty;

    public TaskStatuses Status { get; set; }
}