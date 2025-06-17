using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Dto.Regulator;

[ExcludeFromCodeCoverage]
public class RegistrationMaterialDto
{
    public Guid Id { get; set; } 
    public Guid RegistrationId { get; set; }
    public required string MaterialName { get; set; }
    public int MaterialId { get; set; } 
    public string? Status { get; set; }
    public int StatusId { get; set; }
    public string? StatusUpdatedBy { get; init; }
    public DateTime? StatusUpdatedDate { get; init; }
    public string? ApplicationReferenceNumber { get; init; }
    public string? RegistrationReferenceNumber { get; init; }
    public string? Comments { get; set; }
    public DateTime? DeterminationDate { get; set; }
    public List<RegistrationTaskDto> Tasks { get; set; } = [];
    public int PermitTypeId { get; set; }
    public decimal PPCReprocessingCapacityTonne { get; set; }
    public decimal WasteManagementReprocessingCapacityTonne { get; set; }
    public decimal InstallationReprocessingTonne { get; set; }
    public decimal EnvironmentalPermitWasteManagementTonne { get; set; }
    public decimal MaximumReprocessingCapacityTonne { get; set; }
    public bool IsMaterialRegistered { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<AccreditationDto> Accreditations { get; set; } = [];
}
