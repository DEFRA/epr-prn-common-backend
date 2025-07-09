using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("Public.RegistrationReprocessingIORawMaterialOrProducts")]
    [ExcludeFromCodeCoverage]
    public class RegistrationReprocessingIORawMaterialOrProducts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public int RegistrationReprocessingIOId { get; set; }
        public string RawMaterialOrProductName { get; set; } = string.Empty;
        public decimal TonneValue { get; set; }
        public bool IsInput { get; set; }
    }
}