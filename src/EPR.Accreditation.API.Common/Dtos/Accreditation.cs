using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class Accreditation// : ExternalIdBase
    {
        public Guid? ExternalId { get; set; } // This has a unique key added via the dbcontext
        
        public Enums.OperatorType OperatorTypeId { get; set; }

        [MaxLength(12)]
        public string ReferenceNumber { get; set; } // This has a unique key added via the dbcontext

        public Guid OrganisationId { get; set; }

        public bool? Large { get; set; } // Currently this means is it for above 400 tonnes or not

        public Enums.AccreditationStatus? AccreditationStatusId { get; set; }

        public int? SiteId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Site Site { get; set; }

        public IEnumerable<OverseasReprocessingSite> OverseasReprocessingSites { get; set; }

        public WastePermit WastePermit { get; set; }
    }
}