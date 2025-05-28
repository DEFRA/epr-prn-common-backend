using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class DeterminationDate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        [ForeignKey("RegistrationMaterial")]
        public int RegistrationMaterialId { get; set; }
        public RegistrationMaterial? RegistrationMaterial { get; set; }
        public DateTime? DeterminateDate { get; set; }
    }
}
