using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[ExcludeFromCodeCoverage]
[Table("Public.RegistrationTaskStatusQueryNote")]
public class RegistrationTaskStatusQueryNote
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Note QueryNote { get; set; }
    [ForeignKey("Note")]
    public int QueryNoteId { get; set; }
    public RegulatorRegistrationTaskStatus RegulatorRegistrationTaskStatus { get; set; }
    [ForeignKey("RegulatorRegistrationTaskStatus")]
    public int RegulatorRegistrationTaskStatusId { get; set; }

}
