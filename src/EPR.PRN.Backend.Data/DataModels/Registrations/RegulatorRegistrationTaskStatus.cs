using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegulatorRegistrationTaskStatus")]
public class RegulatorRegistrationTaskStatus: RegulatorTaskStatusBase
{
    public int? RegistrationId { get; set; }
}