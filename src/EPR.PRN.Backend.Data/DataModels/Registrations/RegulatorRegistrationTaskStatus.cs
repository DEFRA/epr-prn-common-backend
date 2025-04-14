using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public class RegulatorRegistrationTaskStatus: RegulatorTaskStatusBase
{
    public int? RegistrationId { get; set; } // Identifier for the specific registration
}