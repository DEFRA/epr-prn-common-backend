using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[Table("Public.AccreditationDulyMade")]
[ExcludeFromCodeCoverage]
public class AccreditationDulyMade
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    [ForeignKey("Accreditation")]
    public int AccreditationId { get; set; }      
    [ForeignKey("TaskStatus")]
    public int TaskStatusId { get; set; }
    public LookupTaskStatus? TaskStatus { get; set; }
    public DateTime? DulyMadeDate { get; set; }
    public Guid? DulyMadeBy { get; set; }
    public AccreditationDeterminationDate? DeterminationDate { get; set; }

  }