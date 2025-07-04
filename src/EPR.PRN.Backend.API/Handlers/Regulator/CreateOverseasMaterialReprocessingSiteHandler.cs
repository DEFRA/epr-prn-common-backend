using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator
{
    public class CreateOverseasMaterialReprocessingSiteHandler(IMaterialRepository repository)
        : IRequestHandler<CreateOverseasMaterialReprocessingSiteCommand>
    {
        public async Task Handle(CreateOverseasMaterialReprocessingSiteCommand command, CancellationToken cancellationToken)
        {           
            await repository.SaveOverseasReprocessingSites(command.UpdateOverseasAddress);
        }
    }
}
