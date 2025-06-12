using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegulatorAccreditationTaskCommand : UpdateRegulatorTaskCommandBase
{
    [Required]
    public required Guid AccreditationId { get; set; }

}