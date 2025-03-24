using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationContact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public int MaterialId { get; set; }

        public int PersonId { get; set; }

        public virtual Registration Registration { get; set; }

        [ForeignKey("MaterialId")]
        public virtual Material Material { get; set; }

        public virtual ICollection<RegistrationContact> RegistrationContacts { get; set; }
    }
}
