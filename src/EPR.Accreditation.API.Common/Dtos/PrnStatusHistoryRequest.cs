using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Accreditation.API.Common.Dtos
{
    /// <summary>
    /// An request to update a PRN's history.
    /// </summary>
    /// <remarks>
    /// Use this when creating new status updates, 
    /// but use <see cref="PrnStatusHistoryResponse"/> for reading data, as it includes the creation date of the record. 
    /// </remarks>
    public class PrnStatusHistoryRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user who changed the PRN's status.
        /// </summary>
        public Guid CreatedByUser { get; init; }

        /// <summary>
        /// Gets or sets the ID of the organisation the user who changed the PRN's status belongs to.
        /// </summary>
        public Guid CreatedByOrganisationId { get; init; }

        /// <summary>
        /// Gets or sets the name of the organisation the user who changed the PRN's status belongs to.
        /// </summary>
        [MaxLength(100)]
        public string OrganisationName { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the status that the PRN was changed to.
        /// </summary>
        public int PrnStatusId { get; init; }

        /// <summary>
        /// Gets or sets any comments about the status change.
        /// </summary>
        [MaxLength(1000)]
        public string Comment { get; init; } = string.Empty;
    }
}
