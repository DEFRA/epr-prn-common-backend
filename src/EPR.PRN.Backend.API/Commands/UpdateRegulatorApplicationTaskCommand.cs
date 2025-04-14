using EPR.PRN.Backend.Data.DataModels.Registrations;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegulatorApplicationTaskCommand : UpdateRegulatorTaskCommandBase
{
    [Required]
    public int registrationMaterialId { get; set; }
    public override int TypeId { get { return registrationMaterialId; } }
}