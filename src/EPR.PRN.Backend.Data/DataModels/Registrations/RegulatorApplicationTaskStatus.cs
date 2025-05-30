using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorApplicationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorApplicationTaskStatus : RegulatorTaskStatusBase
{
    public RegistrationMaterial RegistrationMaterial { get; set; }
    [ForeignKey("RegistrationMaterial")]
    public int? RegistrationMaterialId { get; set; }
    [MaxLength(500)]
    public string? Comments { get; set; }
}