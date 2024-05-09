namespace EPR.Accreditation.API.Common.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class Address
    {
        /// <summary>
        /// Gets or sets the address1 value
        /// </summary>
        public string Address1 { get; set; } // this is a required field, but we don't store it as such becuae save and come back can have incomplete pages

        /// <summary>
        /// Gets or sets address2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets town.
        /// </summary>
        public string Town { get; set; } // this is a required field, but we don't store it as such becuae save and come back can have incomplete pages

        /// <summary>
        /// Gets or sets county.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets postcode.
        /// </summary>
        [MaxLength(8)]
        public string Postcode { get; set; } // this is a required field, but we don't store it as such becuae save and come back can have incomplete pages
    }
}
