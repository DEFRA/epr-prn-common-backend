using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Data.Migrations;
using MediatR;
using static EPR.PRN.Backend.Obligation.Constants.ObligationConstants;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class RegulatorAccreditationMarkAsDulyMadeHandler(
    IRegulatorAccreditationRepository rmRepository
) : IRequestHandler<RegulatorAccreditationMarkAsDulyMadeCommand>
{
    public async Task Handle(RegulatorAccreditationMarkAsDulyMadeCommand request, CancellationToken cancellationToken)
    {
        var accreditationEntity = await rmRepository.GetAccreditationById(request.Id);
        if (accreditationEntity == null)
        {
            throw new KeyNotFoundException("Accreditation not found.");
        }
        await rmRepository.AccreditationMarkAsDulyMade(
            request.Id,
            (int)RegulatorTaskStatus.Completed,
            request.DulyMadeDate,
            request.DeterminationDate,
        request.DulyMadeBy
        );
    }
}
