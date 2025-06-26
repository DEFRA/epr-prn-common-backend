using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [Table("Public.DeterminationDate")]
    [ExcludeFromCodeCoverage]
    public class DeterminationDate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        public RegistrationMaterial? RegistrationMaterial { get; set; }
        public DateTime DeterminateDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IsOverdue { get; private set; } 
        
    }
}
