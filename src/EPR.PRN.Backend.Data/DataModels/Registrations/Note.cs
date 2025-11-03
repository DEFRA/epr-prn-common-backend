using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[ExcludeFromCodeCoverage]
[Table("Public.Note")]
public class Note
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [MaxLength(500)]
    public string Notes { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
}