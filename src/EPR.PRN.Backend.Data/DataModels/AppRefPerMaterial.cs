using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class AppRefPerMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public int RegistrationId { get; set; }

        [Required]
        public int MaterialId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReferenceNo { get; set; }

        [ForeignKey("RegistrationId")]
        public virtual Registration Registration { get; set; }
    }
}