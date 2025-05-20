using System.ComponentModel.DataAnnotations;

namespace EPR.Accreditation.API.Common.Dtos
{
    public class ReprocessorSupportingInformation
    {
        public Enums.ReprocessorSupportingInformationType ReprocessorSupportingInformationTypeId { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }

        public decimal? Tonnes { get; set; }
    }
}