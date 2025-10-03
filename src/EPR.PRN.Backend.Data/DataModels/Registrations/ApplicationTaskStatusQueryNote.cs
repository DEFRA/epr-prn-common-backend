using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
[Table("Public.ApplicationTaskStatusQueryNote")]
public class ApplicationTaskStatusQueryNote
{    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Note Note { get; set; } = null!;
    [ForeignKey("Note")]
    public int QueryNoteId { get; set; }
    public RegulatorApplicationTaskStatus RegulatorApplicationTaskStatus { get; set; } = null!;
    [ForeignKey("RegulatorApplicationTaskStatus")]
    public int RegulatorApplicationTaskStatusId { get; set; }   
}