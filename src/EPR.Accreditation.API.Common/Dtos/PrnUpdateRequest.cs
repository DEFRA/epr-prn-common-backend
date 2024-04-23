using EPR.Accreditation.API.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Accreditation.API.Common.Dtos
{
    /// <summary>
    /// Used for updating a PRN.  Combines PRN details with status update details.
    /// </summary>
    public class PrnUpdateRequest
    {
        public PackageRecyclingNoteRequest Prn { get; init; }
        public PrnStatusHistoryRequest Status { get; init; }
    }
}
