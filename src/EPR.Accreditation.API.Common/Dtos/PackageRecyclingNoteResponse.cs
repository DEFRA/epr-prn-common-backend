using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Dtos
{
    public record PackageRecyclingNoteResponse : PackageRecyclingNoteRequest
    {
        public int Id { get; init; }

        public Guid ExternalId { get; init; }

        /// <summary>
        /// Gets or sets the date and time of when the PRN was created.
        /// </summary>
        public DateTime CreatedDate { get; init; }



        /// <summary>
        /// Gets or sets the date and time when the PRN was last updated.
        /// </summary>
        public DateTime LastUpdatedDate { get; init; }
    }
}