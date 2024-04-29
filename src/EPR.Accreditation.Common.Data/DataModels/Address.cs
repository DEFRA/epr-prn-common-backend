namespace EPR.Accreditation.API.Common.Data.DataModels
{
    using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Data class to represent an address for the organisation
    /// </summary>
    public class Address : IdBaseEntity
    {
        /// <summary>
        /// Gets or sets the address1 value
        /// </summary>
        [Required]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets address2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets town.
        /// </summary>
        [Required]
        public string Town { get; set; }

        /// <summary>
        /// Gets or sets county.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets postcode.
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string Postcode { get; set; } // need to make a unique key

        /// <summary>
        /// Gets or sets the organisation key that the accreditation, and therefore address
        /// is for
        /// </summary>
        public Guid OrganisationId { get; set; } // along with Postcode this is part of a compound unique key

        #region NavigationProperties
        public virtual Site Site { get; set; }

        public virtual Accreditation Accreditation { get; set; }
        #endregion
    }
}
