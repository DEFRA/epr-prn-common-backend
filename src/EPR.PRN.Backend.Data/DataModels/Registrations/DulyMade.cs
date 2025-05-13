using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
public class DulyMade
{
    public int Id { get; set; }   
    [ForeignKey("RegistrationMaterial")]
    public int RegistrationMaterialId { get; set; }
    public required RegistrationMaterial RegistrationMaterial { get; set; }   
    [ForeignKey("TaskStatus")]
    public int TaskStatusId { get; set; }
    public LookupTaskStatus? TaskStatus { get; set; }
    public DateTime? DulyMadeDate { get; set; }
    public Guid? DulyMadeBy { get; set; }
    public DateTime? DeterminationDate { get; set; }
  }