using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using EPR.Accreditation.API.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class WasteCode : IdBaseEntity
    {
        [ForeignKey("AccreditationMaterial")]
        public int AccreditationMaterialId { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        [ForeignKey("WasteCodeType")]
        public Enums.WasteCodeType WasteCodeTypeId { get; set; }

        #region Navigation properties
        public virtual AccreditationMaterial AccreditationMaterial { get; set; }

        public virtual WasteCodeType WasteCodeType { get; set; }
        #endregion
    }
}
