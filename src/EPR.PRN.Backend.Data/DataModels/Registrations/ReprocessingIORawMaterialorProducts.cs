using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[ExcludeFromCodeCoverage]
[Table("Public.RegistrationReprocessingIORawMaterialorProducts")]
public class ReprocessingIORawMaterialorProducts
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [Required]
    public Guid ExternalID { get; set; }
    [ForeignKey("RegistrationReprocessingIO")]
    public int RegistrationReprocessingIOId { get; set; }
    [StringLength(50)]
    public string RawMaterialNameorProductName { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal TonneValue { get; set; }
    public bool IsInput { get; set; }
}