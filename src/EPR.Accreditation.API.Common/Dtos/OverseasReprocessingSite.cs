using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class OverseasReprocessingSite
    {
        [MaxLength(500)]
        public string UkPorts { get; set; }

        [MaxLength(500)]
        public string Outputs { get; set; }

        [MaxLength(500)]
        public string RejectedPlans { get; set; }

        public WastePermit WastePermit { get; set; }

        public virtual OverseasContactPerson OverseasContactPerson { get; set; }

        public OverseasAddress OverseasAddress { get; set; }
    }
}
