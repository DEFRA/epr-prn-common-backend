using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
[Table("Public.ApplicationTaskStatusQueryNotes")]
public class ApplicationTaskStatusQueryNotes
{    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public QueryNote QueryNote { get; set; }
    [ForeignKey("QueryNote")]
    public int QueryNoteId { get; set; }
    public RegulatorApplicationTaskStatus RegulatorApplicationTaskStatus { get; set; }
    [ForeignKey("RegulatorApplicationTaskStatus")]
    public int RegulatorApplicationTaskStatusId { get; set; }   
}