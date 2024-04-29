namespace EPR.Accreditation.API.Common.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class Address
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
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the organisation key that the accreditation, and therefore address
        /// is for
        /// </summary>
        public Guid OrganisationId { get; set; }
    }
}
