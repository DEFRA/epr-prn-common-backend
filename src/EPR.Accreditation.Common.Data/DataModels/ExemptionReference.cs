using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class ExemptionReference : IdBaseEntity
    {
        [ForeignKey("Site")]
        public int SiteId { get; set; }

        public string Reference { get; set; }

        public virtual Site Site { get; set; }
    }
}
