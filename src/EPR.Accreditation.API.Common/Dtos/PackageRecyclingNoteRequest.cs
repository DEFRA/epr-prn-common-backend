using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Dtos
{
    public record PackageRecyclingNoteRequest
    {
        /// <summary>
        /// Gets or sets the type of the PRN.
        /// </summary>
        public Enums.OperatorType? OperatorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the PRN's reference number.
        /// </summary>
        [MaxLength(20)]
        public string ReferenceNumber { get; set; } // This has a unique key added via the dbcontext

        /// <summary>
        /// Gets or sets the ID of the organisation that raised the PRN.
        /// </summary>
        public Guid? OrganisationId { get; set; }

        /// <summary>
        /// Gets or sets the status of the PRN.
        /// </summary>
        public int? PrnStatusId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the site the PRN is for.
        /// </summary>
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who raised the PRN.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ID of the most recent user to update the PRN.
        /// </summary>
        public Guid? LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets any notes about the PRN.
        /// </summary>
        [MaxLength(200)]
        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the ID of the material the PRN is for.
        /// </summary>
        public int? MaterialId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the PRN was created in December.
        /// </summary>
        public bool? IsDecember { get; set; }

        /// <summary>
        /// Gets or sets the quantity in tons of packaging that the PRN is for.
        /// </summary>
        public int? TonnageValue { get; set; }

        /// <summary>
        /// Gets or sets the ID of the producer associated with the PRN.
        /// </summary>
        public int? ProducerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the producer associated with the PRN.
        /// </summary>
        public string ProducerName { get; set; }

        /// <summary>
        /// Gets or sets the PRN's accreditation ID.
        /// </summary>
        public int? AccreditationId { get; set; }

        /// <summary>
        /// Gets or sets the PRN's accreditation reference.
        /// </summary>
        public string AccreditationReference { get; set; } = string.Empty;
    }
}