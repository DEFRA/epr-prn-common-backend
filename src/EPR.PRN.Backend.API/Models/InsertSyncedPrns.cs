using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;

namespace EPR.PRN.Backend.API.Models
{
    [ExcludeFromCodeCoverage]
    public class InsertSyncedPrn
    {
        public string EvidenceNo { get; set; } = null!;

        [AllowedValues(EprnStatus.ACCEPTED, EprnStatus.REJECTED, ErrorMessage = "Only 'ACCEPTED' or 'REJECTED' status are allowed")]
        public EprnStatus EvidenceStatus { get; set; }
    }
}
