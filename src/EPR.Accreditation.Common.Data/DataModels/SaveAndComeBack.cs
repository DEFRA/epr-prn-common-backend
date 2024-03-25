using EPR.Accreditation.API.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Accreditation.API.Common.Data.DataModels
{
    public class SaveAndComeBack : IdBaseEntity
    {
        [MaxLength(30)]
        public string Area { get; set; }

        [MaxLength(30)]
        public string Controller { get; set; }

        [MaxLength(30)]
        public string Action { get; set; }

        public string Parameters { get; set; }

        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }

        #region Navigation properties
        public virtual Accreditation Accreditation { get; set; }

        #endregion
    }
}
