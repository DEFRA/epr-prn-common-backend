using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
[Table("Public.AccreditationTaskStatusQueryNote")]
public class AccreditationTaskStatusQueryNote
{    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Note Note { get; set; }
    [ForeignKey("Note")]
    public int QueryNoteId { get; set; }
    public RegulatorAccreditationTaskStatus RegulatorAccreditationTaskStatus { get; set; }
    [ForeignKey("RegulatorAccreditationTaskStatus")]
    public int RegulatorAccreditationTaskStatusId { get; set; }   
}