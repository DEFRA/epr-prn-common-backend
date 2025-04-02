﻿using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Common.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationOverviewDto
{
    public int Id { get; set; }  // Registration.Id
    public string OrganisationName { get; set; }
    public string OrganisationType { get; set; }
    public string Regulator { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; }
    public List<RegistrationMaterialDto> Materials { get; set; }
}    
