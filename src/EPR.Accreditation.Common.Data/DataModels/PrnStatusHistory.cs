namespace EPR.Accreditation.API.Common.Data.DataModels
{
    using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the database table that stores records of changes to the PRNs' statuses.
    /// </summary>
    [Table("PrnStatusHistory")]
    public class PrnStatusHistory: IdBaseEntity
    {
        /// <summary>
        /// Gets or sets the date the status was changed.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who changed the PRN's status.
        /// </summary>
        public Guid CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the ID of the organisation the user who changed the PRN's status belongs to.
        /// </summary>
        public Guid CreatedByOrganisationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the organisation the user who changed the PRN's status belongs to.
        /// </summary>
        [MaxLength(100)]
        public string OrganisationName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status that the PRN was changed to.
        /// </summary>
        [ForeignKey(nameof(PrnStatus))]
        public int PrnStatusId { get; set; }

        /// <summary>
        /// Gets or sets the PRN who's status was changed.
        /// </summary>
        [ForeignKey(nameof(PackageRecyclingNote))]
        public int PrnId { get; set; }

        /// <summary>
        /// Gets or sets any comments about the status change.
        /// </summary>
        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;


        #region Navigation properties

        public virtual PrnStatus PrnStatus { get; set; }

        public virtual PackageRecyclingNote Prn { get; set; }

        #endregion Navigation properties


    }
}
