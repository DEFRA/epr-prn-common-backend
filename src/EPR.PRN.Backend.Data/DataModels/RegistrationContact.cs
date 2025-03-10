using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationContact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public string MaterialId { get; set; }

        public int PersonId { get; set; }

        public virtual Registration Registration { get; set; } = null!;

        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; } = null!;

        public virtual ICollection<RegistrationContact> RegistrationContacts { get; set; } = null!;
    }
}
