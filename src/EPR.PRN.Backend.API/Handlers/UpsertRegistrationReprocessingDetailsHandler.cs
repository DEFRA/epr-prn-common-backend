using AutoMapper;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;
public class UpsertRegistrationReprocessingDetailsHandler(IRegistrationMaterialRepository repository, IMapper mapper)
    : IRequestHandler<RegistrationReprocessingIOCommand, RegistrationReprocessingIOResponseDto>
{
    public async Task<RegistrationReprocessingIOResponseDto> Handle(RegistrationReprocessingIOCommand command, CancellationToken cancellationToken)
    {
       var registrationReprocessingIO = mapper.Map<RegistrationReprocessingIO>(command);

       return await repository.UpsertRegistrationReprocessingDetailsAsync(command.RegistrationMaterialId, registrationReprocessingIO);
    }
}