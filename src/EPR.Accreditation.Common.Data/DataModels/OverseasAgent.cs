using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class OverseasAgent : IdBaseEntity
    {
        [ForeignKey("OverseasAddress")]
        public int OverseasAddressId { get; set; }

        [ForeignKey("OverseasReprocessingSite")]
        public int OverseasReprocessingSiteId { get; set; }

        [MaxLength(100)]
        public string Fullname { get; set; }

        [MaxLength(100)]
        public string Position { get; set; }

        [MaxLength(30)]
        public string Telephone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        #region Navigation properties
        public virtual OverseasAddress OverseasAddress { get; set; }

        public virtual OverseasReprocessingSite OverseasReprocessingSite { get; set; }
        #endregion
    }
}