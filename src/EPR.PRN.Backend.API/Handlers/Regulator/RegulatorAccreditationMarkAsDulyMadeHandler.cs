using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

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
            request.DeterminationDate,
            request.DulyMadeDate,
             request.DulyMadeBy
        );
    }
}
