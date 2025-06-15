using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

/// <summary>
/// Handler for the <see cref="GetRegistrationByOrganisationQuery"/>.
/// </summary>
/// <param name="registrationRepository">Repository for handling registrations.</param>
public class GetRegistrationByOrganisationQueryHandler(
    IRegistrationRepository registrationRepository
) : IRequestHandler<GetRegistrationByOrganisationQuery, RegistrationDto?>
{
    /// <inheritdoc />>.
    public async Task<RegistrationDto?> Handle(GetRegistrationByOrganisationQuery request, CancellationToken cancellationToken)
    {
        var result = await registrationRepository.GetByOrganisationAsync(request.ApplicationTypeId, request.OrganisationId);
        if (result is null)
        {
            return null;
        }

        var mapped = new RegistrationDto
        {
            Id = result.Id,
            ExternalId = result.ExternalId,
            ApplicationTypeId = result.ApplicationTypeId,
            OrganisationId = result.OrganisationId,
            RegistrationStatusId = result.RegistrationStatusId,
            BusinessAddress = MapAddress(result.BusinessAddress),
            ReprocessingSiteAddress = MapAddress(result.ReprocessingSiteAddress),
            LegalDocumentAddress = MapAddress(result.LegalDocumentAddress)
        };

        if (result.ApplicantRegistrationTasksStatus is not null)
        {
            mapped.Tasks = result.ApplicantRegistrationTasksStatus.Select(t => new RegistrationTaskDto
            {
                Id = t.ExternalId,
                Status = t.TaskStatus.Name,
                TaskName = t.Task.Name
            }).ToList();
        }
    
        return mapped;
    }

    private static AddressDto? MapAddress(Address? address)
    {
        if (address is null)
        {
            return null;
        }

        return new AddressDto
        {
            Id = address.Id,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            TownCity = address.TownCity,
            County = address.County,
            PostCode = address.PostCode,
            GridReference = address.GridReference,
            NationId = address.NationId.GetValueOrDefault()
        };
    }
}