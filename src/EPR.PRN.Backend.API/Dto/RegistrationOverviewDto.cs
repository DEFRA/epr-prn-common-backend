using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
/// <summary>
/// Top-level DTO returned by GET /registrations/{id}.
/// </summary>
public class RegistrationOverviewDto
{
    public int Id { get; set; }  // Registration.Id
    public string OrganisationName { get; set; }
    public string OrganisationType { get; set; }
    public string Regulator { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; }
    public List<RegistrationMaterialDto> Materials { get; set; }
}    
