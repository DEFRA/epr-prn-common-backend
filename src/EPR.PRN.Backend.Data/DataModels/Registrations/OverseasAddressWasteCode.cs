using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    [Table("Public.OverseasAddressWasteCodes")]
    public class OverseasAddressWasteCode
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public OverseasAddress? OverseasAddress { get; set; }
        [ForeignKey("OverseasAddressId")]
        public int OverseasAddressId { get; set; }
        [MaxLength(10)]
        public required string CodeName { get; set; }
    }
}
