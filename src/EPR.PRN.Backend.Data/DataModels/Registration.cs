using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        public Guid OrganisationId { get; set; }

        public int ApplicationTypeId { get; set; }

        public int RegistrationStatusId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public Guid UpdatedBy { get; set; }

        public int BusinessAddressId { get; set; }

        public int ReprocessingSiteAddressId { get; set; }

        public int LegalDocumentAddressId { get; set; }
    }
}
