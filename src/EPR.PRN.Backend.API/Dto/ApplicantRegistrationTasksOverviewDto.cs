using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.API.Dto;

[ExcludeFromCodeCoverage]
public class ApplicantRegistrationTasksOverviewDto
{
    public Guid Id { get; set; }

    public Guid OrganisationId { get; set; } 

    public List<ApplicantRegistrationTaskDto> Tasks { get; set; } = [];

    public List<ApplicantRegistrationMaterialTaskOverviewDto> Materials { get; set; } = [];
}

[ExcludeFromCodeCoverage]
public class ApplicantRegistrationTaskDto
{
    public Guid? Id { get; set; }
    public required string TaskName { get; set; }
    public required string Status { get; set; }
}