using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.ExporterJourney;

public class UpsertAddressForServiceOfNoticesHandler(IRegistrationRepository registrationRepository)
    : IRequestHandler<UpsertAddressForServiceOfNoticesCommand>
{
    public async Task Handle(UpsertAddressForServiceOfNoticesCommand request, CancellationToken cancellationToken)
    {
        await registrationRepository.UpsertLegalDocumentAddress(request.RegistrationId, request.Dto.LegalDocumentAddress);
    }
}