using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public class RegulatorApplicationTaskStatus : RegulatorTaskStatusBase
{
    public int? RegistrationMaterialId { get; set; } // Identifier for the registration material associated with the task
}