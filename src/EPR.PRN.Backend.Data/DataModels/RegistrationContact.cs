using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RegistrationContact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public int RegistrationId { get; set; }

        public int MaterialId { get; set; }

        public int PersonId { get; set; }
    }
}
