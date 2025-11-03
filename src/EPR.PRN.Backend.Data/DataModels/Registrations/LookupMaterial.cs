using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Lookup.Material")]
[ExcludeFromCodeCoverage]
public class LookupMaterial
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(250)]
    public string MaterialName { get; set; } = string.Empty;
    [Required]
    [MaxLength(250)]
    public string MaterialCode { get; set; } = string.Empty;
}