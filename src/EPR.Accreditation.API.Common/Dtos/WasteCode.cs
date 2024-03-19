using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class WasteCode
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        public Enums.WasteCodeType WasteCodeTypeId { get; set; }
    }
}
