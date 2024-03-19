using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class SiteAuthority : IdBaseEntity
    {
        public Guid UserId { get; set; } // A selected user id for authorised for the site. The entire list for the org will come from Accounts API

        [ForeignKey("Site")]
        public int SiteId { get; set; }

        #region Navigation properties
        public virtual Site Site { get; set; }
        #endregion
    }
}
