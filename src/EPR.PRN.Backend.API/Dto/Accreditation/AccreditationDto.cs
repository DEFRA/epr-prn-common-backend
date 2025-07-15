using EPR.PRN.Backend.Data.DTO.Accreditiation;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto.Accreditation
{
    [ExcludeFromCodeCoverage]
    public class AccreditationDto : AccreditationDtoBase
    {
        public string MaterialName { get; set; }
    }
}
