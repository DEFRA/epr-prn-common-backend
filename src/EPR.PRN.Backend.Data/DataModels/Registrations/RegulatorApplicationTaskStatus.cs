using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorApplicationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorApplicationTaskStatus : RegulatorTaskStatusBase
{
    public int? RegistrationMaterialId { get; set; }
}