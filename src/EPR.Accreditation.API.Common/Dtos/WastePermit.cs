using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class WastePermit
    {
        public int Id { get; set; }

        public int AccreditationId { get; set; }

        public int? OverseasReprocessingSiteId { get; set; }

        [MaxLength(100)]
        public string DealerRegistrationNumber { get; set; }

        [MaxLength(100)]
        public string EnvironmentalPermitNumber { get; set; }

        [MaxLength(10)]
        public string PartAActivityReferenceNumber { get; set; }

        [MaxLength(10)]
        public string PartBActivityReferenceNumber { get; set; }

        [MaxLength(50)]
        public string DischargeConsentNumber { get; set; }

        public bool? WastePermitExemption { get; set; }
    }
}
