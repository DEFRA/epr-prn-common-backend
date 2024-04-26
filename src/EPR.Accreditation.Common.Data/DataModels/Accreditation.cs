using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class Accreditation : IdBaseEntity
    {
        public Guid ExternalId { get; set; } // This has a unique key added via the dbcontext

        [ForeignKey("OperatorType")]
        public Enums.OperatorType OperatorTypeId { get; set; }

        [MaxLength(12)]
        public string ReferenceNumber { get; set; }

        public Guid OrganisationId { get; set; }

        public bool? Large { get; set; } // Currently this means is it for above 400 tonnes or not

        [ForeignKey("AccreditationStatus")]
        public Enums.AccreditationStatus AccreditationStatusId { get; set; }

        [ForeignKey("Site")]
        public int? SiteId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        #region Navigation properties
        public virtual Site Site { get; set; }

        public virtual WastePermit WastePermit { get; set; }

        public virtual OperatorType OperatorType { get; set; }

        public virtual AccreditationStatus AccreditationStatus { get; set; }

        public virtual ICollection<AccreditationTaskProgress> TaskProgress { get; set; }

        public virtual ICollection<OverseasReprocessingSite> OverseasReprocessingSites { get; set; }

        public virtual ICollection<FileUpload> FileUploads { get; set; }
        #endregion
    }
}