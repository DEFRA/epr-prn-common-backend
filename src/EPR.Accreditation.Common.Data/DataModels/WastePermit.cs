using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class WastePermit : IdBaseEntity
    {
        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; } // this is a unique key

        [ForeignKey("OverseasReprocessingSite")]
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

        #region Navigation properties
        public Accreditation Accreditation { get; set; }

        public OverseasReprocessingSite OverseasReprocessingSite { get; set; }
        #endregion
    }
}
