using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorAccreditationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorAccreditationTaskStatus : RegulatorTaskStatusBase
{
    public int? AccreditationId { get; set; }
}