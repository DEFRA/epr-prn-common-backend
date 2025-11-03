using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[Table("Public.DulyMade")]
[ExcludeFromCodeCoverage]
public class DulyMade
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }
    [ForeignKey("RegistrationMaterial")]
    public int RegistrationMaterialId { get; set; }
    public RegistrationMaterial? RegistrationMaterial { get; set; }
    public DateTime DulyMadeDate { get; set; }
    public Guid DulyMadeBy { get; set; }
}