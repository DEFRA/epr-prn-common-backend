using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class Material : IdBaseEntity
    {
        public Guid? ExternalId { get; set; }

        public string English { get; set; }

        public string Welsh { get; set; }

        #region Navigation properties
        public virtual ICollection<AccreditationMaterial> AccreditationMaterials { get; set; }
        #endregion
    }
}