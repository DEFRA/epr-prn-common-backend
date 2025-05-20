namespace EPR.Accreditation.API.Common.Data.DataModels
{
    using System.ComponentModel.DataAnnotations.Schema;
    using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;

    public class Site : IdBaseEntity
    {
        // Although a one to one relationship with Accreditation, in future Sites get created first
        // so this field will be required
        public Guid ExternalId { get; set; } // unique key created in DbContext

        [ForeignKey("Address")]
        public int AddressId { get; set; }

        #region Navigation properties
        public virtual Address Address { get; set; }

        public virtual ICollection<Accreditation> Accreditations { get; set; }

        public virtual ICollection<AccreditationMaterial> AccreditationMaterials { get; set; }

        public virtual ICollection<ExemptionReference> ExemptionReferences { get; set; }

        public virtual ICollection<SiteAuthority> SiteAuthorities { get; set; }
        #endregion
    }
}
