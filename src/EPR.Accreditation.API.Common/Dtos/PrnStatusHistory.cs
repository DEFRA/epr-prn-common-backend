using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class PrnStatusHistory
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
    }
}
