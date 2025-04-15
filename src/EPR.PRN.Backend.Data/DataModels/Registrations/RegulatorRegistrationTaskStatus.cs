using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public class RegulatorRegistrationTaskStatus: RegulatorTaskStatusBase
{
    public int? RegistrationId { get; set; } // Identifier for the specific registration
}