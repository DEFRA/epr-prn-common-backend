using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class RegistrationDto
{
    public Guid Id { get; set; }

    public int ApplicationTypeId { get; set; }
    
    public Guid OrganisationId { get; set; }
    
    public int RegistrationStatusId { get; set; }
    
    public AddressDto? BusinessAddress { get; set; }

    public AddressDto? ReprocessingSiteAddress { get; set; }

    public AddressDto? LegalDocumentAddress { get; set; }

    public IList<RegistrationTaskDto> Tasks { get; set; } = [];
}