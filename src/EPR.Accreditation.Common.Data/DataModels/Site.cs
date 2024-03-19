using System.ComponentModel.DataAnnotations;
using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class Site : IdBaseEntity
    {
        public Guid ExternalId { get; set; } // unique key created in DbContext

        [Required]
        [MaxLength(100)]
        public string Address1 { get; set; }

        [MaxLength(100)]
        public string Address2 { get; set; }

        [Required]
        [MaxLength(100)]
        public string Town { get; set; }

        [MaxLength(100)]
        public string County { get; set; }

        [Required]
        [MaxLength(10)]
        public string Postcode { get; set; } // along with OrganisationId this is part of a compound unique key

        public Guid OrganisationId { get; set; } // along with Postcode this is part of a compound unique key

        #region Navigation properties
        public virtual ICollection<Accreditation> Accreditations { get; set; }

        public virtual ICollection<AccreditationMaterial> AccreditationMaterials { get; set; }

        public virtual ICollection<ExemptionReference> ExemptionReferences { get; set; }

        public virtual ICollection<SiteAuthority> SiteAuthorities { get; set; }
        #endregion
    }
}
