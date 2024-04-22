namespace EPR.Accreditation.API.Common.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public class WasteCode
    {
        [MaxLength(50)]
        public string Code { get; set; }

        public Enums.WasteCodeType WasteCodeTypeId { get; set; }
    }
}
