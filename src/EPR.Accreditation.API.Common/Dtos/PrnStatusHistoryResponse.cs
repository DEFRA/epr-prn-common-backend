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
    /// Used for records retrieved from the PrnStatusHistory database table.
    /// </summary>
    public class PrnStatusHistoryResponse :PrnStatusHistoryRequest
    {
        /// <summary>
        /// Gets or sets the date the status was changed.
        /// </summary>
        public DateTime CreatedOn { get; init; }
    }
}
