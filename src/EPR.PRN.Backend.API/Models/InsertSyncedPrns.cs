using EPR.PRN.Backend.API.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Models
{
    public class InsertSyncedPrn
    {
        public string EvidenceNo { get; set; } = null!;

        [AllowedValues(EprnStatus.ACCEPTED, EprnStatus.REJECTED, ErrorMessage = "Only 'ACCEPTED' or 'REJECTED' status are allowed")]
        public EprnStatus EvidenceStatus { get; set; }
    }
}
