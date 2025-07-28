using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpsertInterimSiteHandler(IMaterialRepository materialRepository) : IRequestHandler<UpsertInterimSiteCommand>
{
    public async Task Handle(UpsertInterimSiteCommand command, CancellationToken cancellationToken)
    {
        foreach (var parentSite in command.InterimSitesRequestDto!.OverseasMaterialReprocessingSites)
        {
            var parentExternalId = parentSite.OverseasAddressId;

            if (parentSite.InterimSiteAddresses != null)
            {
                foreach (var interim in parentSite.InterimSiteAddresses)
                {
                    interim.ParentExternalId = parentExternalId;
                }
            }
        }
        await materialRepository.SaveInterimSitesAsync(command.InterimSitesRequestDto);
    }
}