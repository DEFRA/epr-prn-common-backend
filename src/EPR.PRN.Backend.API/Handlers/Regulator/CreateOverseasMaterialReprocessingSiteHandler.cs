using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator
{
    public class CreateOverseasMaterialReprocessingSiteHandler(IRegistrationMaterialRepository repository)
        : IRequestHandler<CreateOverseasMaterialReprocessingSiteCommand>
    {
        public async Task Handle(CreateOverseasMaterialReprocessingSiteCommand command, CancellationToken cancellationToken)
        {
           throw new NotImplementedException("This handler is not implemented yet. Please implement the logic to create overseas material reprocessing site.");
            // await repository.CreateOverseasMaterialReprocessingSiteAsync(command.OverseasAddresses, command.RegistrationMaterialId);
        }
    }
}
