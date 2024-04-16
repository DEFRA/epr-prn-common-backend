using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class PackageRecyclingNote : IdBaseEntity
    {
        public Guid ExternalId { get; set; } // This has a unique key added via the dbcontext

        [ForeignKey("OperatorType")]
        public Enums.OperatorType PrnTypeId { get; set; }

        [MaxLength(12)]
        public string ReferenceNumber { get; set; }

        public Guid OrganisationId { get; set; }

        [ForeignKey("Site")]
        public int? SiteId { get;set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? LastUpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        #region Navigation properties
        public virtual Site Site { get; set; }

        #endregion
    }
}