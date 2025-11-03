using AutoMapper;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpsertRegistrationReprocessingDetailsHandler(IMaterialRepository repository, IMapper mapper)
    : IRequestHandler<RegistrationReprocessingIOCommand>
{
    public async Task Handle(RegistrationReprocessingIOCommand command, CancellationToken cancellationToken)
    {
        var registrationReprocessingIO = mapper.Map<RegistrationReprocessingIO>(command);

        await repository.UpsertRegistrationReprocessingDetailsAsync(command.RegistrationMaterialId, registrationReprocessingIO);
    }
}