using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class OverseasReprocessingSite : IdBaseEntity
    {
        public Guid ExternalId { get; set; }

        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }

        [ForeignKey("OverseasAddress")]
        public int? OverseasAddressId { get; set; } // unique key as this is a one to one relationship

        [MaxLength(500)]
        public string UkPorts { get; set; }

        [MaxLength(500)]
        public string Outputs { get; set; }

        [MaxLength(500)]
        public string RejectedPlans { get; set; }

        #region Navigation properties
        public virtual Accreditation Accreditation { get; set; }

        public virtual OverseasAddress OverseasAddress { get; set; }

        public virtual OverseasAgent OverseasAgent { get; set; }

        // this will probably go in the end - Exporters are unlikely to need WastePermit info.
        // but for now, it is in the journey
        public virtual WastePermit WastePermit { get; set; }

        public virtual ICollection<AccreditationMaterial> AccreditationMaterials { get; set; }
        #endregion
    }
}