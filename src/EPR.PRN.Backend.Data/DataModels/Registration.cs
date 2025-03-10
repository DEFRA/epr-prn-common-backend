using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? BusinessAddressId { get; set; }

        public int? ReprocessingSiteAddressId { get; set; }

        public int? LegalDocumentAddressId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; } = null!;

        [ForeignKey("RegistrationStatusId")]
        public virtual RegistrationStatus RegistrationStatus { get; set; } = null!;

        [ForeignKey("BusinessAddressId")]
        public virtual Address BusinessAddress { get; set; } = null!;

        [ForeignKey("ReprocessingSiteAddressId")]
        public virtual Address ReprocessingSiteAddress { get; set; } = null!;

        [ForeignKey("LegalDocumentAddressId")]
        public virtual Address LegalDocumentAddress { get; set; } = null!;

        public virtual ICollection<FileUpload> FileUploads { get; set; } = null!;

        public virtual ICollection<AppRefPerMaterial> AppRefPerMaterials { get; set; } = null!;

        public virtual ICollection<RegistrationMaterial> RegistrationMaterials { get; set; } = null!;
    } 
}
