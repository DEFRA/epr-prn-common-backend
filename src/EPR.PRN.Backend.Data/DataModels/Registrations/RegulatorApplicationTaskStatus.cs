using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorApplicationTaskStatus")]
public class RegulatorApplicationTaskStatus : RegulatorTaskStatusBase
{
    public int? RegistrationMaterialId { get; set; }
}