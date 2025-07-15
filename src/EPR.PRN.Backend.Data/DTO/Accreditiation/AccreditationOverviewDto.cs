
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO.Accreditiation
{
    [ExcludeFromCodeCoverage]
    public class AccreditationOverviewDto : AccreditationDtoBase
    {
        public RegistrationMaterialDto? RegistrationMaterial { get; set; }
    }
}
