using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class MaterialPermitType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int NationId { get; set; }
    }
}