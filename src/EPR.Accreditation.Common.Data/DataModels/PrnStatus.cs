namespace EPR.Accreditation.API.Common.Data.DataModels
{
    using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the database table that holds the list of statuses that can be applied to PRNs (Draft, Accepted, Cancelled, etc.) 
    /// </summary>
    [Table("PrnStatus")]
    public class PrnStatus: IdBaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        [MaxLength(20)]
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets a description of the status.
        /// </summary>
        [MaxLength(50)]
        public string StatusDescription { get; set; }
    }
}
