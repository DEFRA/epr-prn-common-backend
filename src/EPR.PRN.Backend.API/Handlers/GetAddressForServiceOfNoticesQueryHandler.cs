using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
using Polly;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handler for the <see cref="GetAllMaterialsQuery"/>.
/// </summary>
/// <param name="materialService">Service for handling materials.</param>
public class GetAddressForServiceOfNoticesQueryHandler(
    IRegistrationRepository registrationRepository
) : IRequestHandler<GetAddressForServiceOfNoticesQuery, GetAddressForServiceOfNoticesDto>
{
    /// <inheritdoc />>.
    public async Task<GetAddressForServiceOfNoticesDto> Handle(GetAddressForServiceOfNoticesQuery request, CancellationToken cancellationToken)
    {
        var registration = await registrationRepository.GetRegistrationByExternalId(request.RegistrationId, cancellationToken);

        if (registration is null)
        {
            throw new KeyNotFoundException("Registration not found.");
        }

        var retVal = new GetAddressForServiceOfNoticesDto
        {
            RegistrationId = request.RegistrationId,
            LegalDocumentAddress = null,
        };

        var legalDocumentAddress = registration.LegalDocumentAddress;
        if (legalDocumentAddress == null && registration.LegalDocumentAddressId != null)
        {
            legalDocumentAddress = await registrationRepository.GetLegalDocumentAddress(request.RegistrationId);
        }

        if (legalDocumentAddress != null)
        {
            retVal.LegalDocumentAddress = new AddressDto
            {
                Id = legalDocumentAddress.Id, 
                AddressLine1 = legalDocumentAddress.AddressLine1,
                AddressLine2 = legalDocumentAddress.AddressLine2,
                TownCity = legalDocumentAddress.TownCity,
                County = legalDocumentAddress.County,
                PostCode = legalDocumentAddress.PostCode,
                NationId = legalDocumentAddress.NationId,
                GridReference = legalDocumentAddress.GridReference, 
            };
        }

        return retVal;
    }
}