using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class OverseasReprocessingSite
    {
        public Guid? ExternalId { get; set; } // This has a unique key added via the dbcontext

        public int AccreditationId { get; set; }

        public int? OverseasAddressId { get; set; } // unique key as this is a one to one relationship

        [MaxLength(500)]
        public string UkPorts { get; set; }

        [MaxLength(500)]
        public string Outputs { get; set; }

        [MaxLength(500)]
        public string RejectedPlans { get; set; }

        public WastePermit WastePermit { get; set; }

        public OverseasAgent OverseasAgent { get; set; }

        public OverseasAddress OverseasAddress { get; set; }
    }
}
