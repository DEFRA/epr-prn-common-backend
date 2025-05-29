using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
[Table("Public.RegistrationTaskStatusQueryNotes")]
public class RegistrationTaskStatusQueryNotes
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public QueryNote QueryNote { get; set; }
    [ForeignKey("QueryNote")]
    public int QueryNoteId { get; set; }
    public RegulatorRegistrationTaskStatus RegulatorRegistrationTaskStatus { get; set; }
    [ForeignKey("RegistrationTaskStatus")]
    public int RegistrationTaskStatusId { get; set; }

}
