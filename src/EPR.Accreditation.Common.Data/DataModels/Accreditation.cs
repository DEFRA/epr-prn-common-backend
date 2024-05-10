namespace EPR.Accreditation.API.Common.Data.DataModels
{
    using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
    using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Accreditation class.
    /// </summary>
    public class Accreditation : IdBaseEntity
    {
        /// <summary>
        /// Gets or sets ExternalId.
        /// </summary>
        public Guid ExternalId { get; set; } // This has a unique key added via the dbcontext

        /// <summary>
        /// Gets or sets OperatoryTypeId
        /// </summary>
        [ForeignKey("OperatorType")]
        public Enums.OperatorType OperatorTypeId { get; set; }

        /// <summary>
        /// Gets or sets ReferenceNumber.
        /// </summary>
        [MaxLength(12)]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets OrganisationId.
        /// </summary>
        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Gets or sets Large.
        /// </summary>
        public bool? Large { get; set; } // Currently this means is it for above 400 tonnes or not

        /// <summary>
        /// Gets or sets LargeFee.
        /// </summary>
        [Column(TypeName = "decimal(10,3)")]
        public decimal? LargeFee { get; set; }

        /// <summary>
        /// Gets or sets AccrediationStausId.
        /// </summary>
        [ForeignKey("AccreditationStatus")]
        public Enums.AccreditationStatus AccreditationStatusId { get; set; }

        /// <summary>
        /// Gets or sets SiteId.
        /// </summary>
        [ForeignKey("Site")]
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets LegalAddressId.
        /// </summary>
        [ForeignKey("Address")]
        public int? LegalAddressId { get; set; }

        /// <summary>
        /// Gets or sets CreatedBy.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets CreatedOn.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets UpdatedBy.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets UpdatedOn.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets AccrediationFee.
        /// </summary>
        public decimal? AccreditationFee { get; set; }

        /// <summary>
        /// Gets or sets CountryCode.
        /// </summary>
        [MaxLength(3)]
        public string CountryCode { get; set; }

        #region Navigation properties
        public virtual Site Site { get; set; }

        public virtual ICollection<AccreditationMaterial> AccreditationMaterials { get; set; }

        public virtual WastePermit WastePermit { get; set; }

        public virtual OperatorType OperatorType { get; set; }

        public virtual AccreditationStatus AccreditationStatus { get; set; }

        public virtual ICollection<AccreditationTaskProgress> TaskProgress { get; set; }

        public virtual ICollection<OverseasReprocessingSite> OverseasReprocessingSites { get; set; }

        public virtual ICollection<FileUpload> FileUploads { get; set; }

        public virtual Address LegalAddress { get; set; }

        #endregion
    }
}