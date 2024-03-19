using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class OverseasAddress : IdBaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        #region Navigation properties
        public virtual Country Country { get; set; }

        public virtual OverseasReprocessingSite OverseasReprocessingSite { get; set; }

        public virtual OverseasAgent OverseasAgent { get; set; }
        #endregion
    }
}