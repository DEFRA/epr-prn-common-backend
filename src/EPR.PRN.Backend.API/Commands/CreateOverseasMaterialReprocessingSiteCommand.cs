using EPR.PRN.Backend.Data.DTO;
using MediatR;
namespace EPR.PRN.Backend.API.Commands
{
    public class CreateOverseasMaterialReprocessingSiteCommand : IRequest
    {
        public UpdateOverseasAddressDto UpdateOverseasAddress { get; set; }        
    }
}
