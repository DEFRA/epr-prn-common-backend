using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorRegistrationTaskStatus")]
[ExcludeFromCodeCoverage]
public class RegulatorRegistrationTaskStatus: RegulatorTaskStatusBase
{
    public int? RegistrationId { get; set; }
}