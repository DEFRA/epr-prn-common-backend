using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    /// <summary>
    /// Implements the database table for storing PRN records.
    /// </summary>
    /// <remarks>
    /// This class doesn't follow the usual naming pattern due to PRN being a reserved file name in Windows,
    /// which causes Git to be unable to see files with that name properly.
    /// </remarks>
    [Table("Prn")]
    public class PackageRecyclingNote : IdBaseEntity
    {
        /// <summary>
        /// Gets or sets the external ID of the PRN.
        /// </summary>
        public Guid ExternalId { get; set; } // This has a unique key added via the dbcontext

        /// <summary>
        /// Gets or sets the type of the PRN.
        /// </summary>
        [ForeignKey(nameof(OperatorType))]
        public Enums.OperatorType PrnTypeId { get; set; }

        ///// <summary>
        ///// Gets or sets the PRN's reference number.
        ///// </summary>
        [MaxLength(20)]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the ID of the organisation that raised the PRN.
        /// </summary>
        public Guid OrganisationId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the site the PRN is for.
        /// </summary>
        [ForeignKey(nameof(Site))]
        public int SiteId { get;set; }

        /// <summary>
        /// Gets or sets the ID of the user who raised the PRN.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time of when the PRN was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ID of the most recent user to update the PRN.
        /// </summary>
        public Guid LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the status of the PRN.
        /// </summary>
        [ForeignKey(nameof(PrnStatus))]
        public int? PrnStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the PRN was last updated.
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets any notes about the PRN.
        /// </summary>
        [MaxLength(200)]
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the material the PRN is for.
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the PRN was created in December.
        /// </summary>
        public bool IsDecember { get; set; }

        /// <summary>
        /// Gets or sets the quantity in tons of packaging that the PRN is for.
        /// </summary>
        public int TonnageValue { get; set; }

        /// <summary>
        /// Gets or sets the ID of the producer associated with the PRN.
        /// </summary>
        public int ProducerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the producer associated with the PRN.
        /// </summary>
        public string ProducerName { get; set; }

        /// <summary>
        /// Gets or sets the PRN's accreditation ID.
        /// </summary>
        public int AccreditationId { get; set; }

        /// <summary>
        /// Gets or sets the PRN's accreditation reference.
        /// </summary>
        public string AccreditationReference { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the PRN is active.
        /// </summary>
        public bool IsActive { get; set; }

        #region Navigation properties

        public virtual Site Site { get; set; }

        public virtual PrnStatus PrnStatus { get; set; }

        public virtual OperatorType PrnType { get; set; }

        #endregion Navigation properties
    }
}