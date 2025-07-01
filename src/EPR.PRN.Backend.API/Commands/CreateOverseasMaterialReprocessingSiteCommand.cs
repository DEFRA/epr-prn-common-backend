using EPR.PRN.Backend.API.Dto.Exporter;
using MediatR;

namespace EPR.PRN.Backend.API.Commands
{
    public class CreateOverseasMaterialReprocessingSiteCommand : IRequest
    {
        List<OverseasAddressDto> OverseasAddresses { get; set; } = [];
        public Guid RegistrationMaterialId { get; set; }
    }
}
