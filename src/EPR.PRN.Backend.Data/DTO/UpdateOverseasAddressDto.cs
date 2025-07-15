using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DTO
{
    [ExcludeFromCodeCoverage]
    public class UpdateOverseasAddressDto
    {
        public List<OverseasAddressDto> OverseasAddresses { get; set; } = [];
        public Guid RegistrationMaterialId { get; set; }
    }
}
