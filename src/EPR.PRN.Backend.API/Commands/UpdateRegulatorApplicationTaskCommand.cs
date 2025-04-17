using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegulatorApplicationTaskCommand : UpdateRegulatorTaskCommandBase
{
    [Required]
    public int RegistrationMaterialId { get; set; }
}