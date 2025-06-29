using EPR.PRN.Backend.API.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegistrationMaterial")]
[ExcludeFromCodeCoverage]
public class RegistrationMaterial
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }

    [ForeignKey("Registration")]
    public int RegistrationId { get; set; }
    public Registration Registration { get; set; }
    [ForeignKey("Material")]
    public int MaterialId { get; set; }
    public LookupMaterial Material { get; set; }
    public LookupRegistrationMaterialStatus? Status { get; set; }
    [ForeignKey("Status")]
    public int? StatusId { get; set; }
    public string? RegistrationReferenceNumber { get; set; } = null;
    public string? Comments { get; set; } = string.Empty;        
    public DateTime StatusUpdatedDate { get; set; }
    public Guid? StatusUpdatedBy { get; set; } 
    public string? ReasonforNotreg { get; set; } = string.Empty;
    public LookupMaterialPermit? PermitType { get; set; }
    [ForeignKey("PermitType")]
    public int PermitTypeId { get; set; }
    [MaxLength(20)]
    public string? PPCPermitNumber { get; set; }
    [MaxLength(20)]
    public string? WasteManagementLicenceNumber { get; set; }
    [MaxLength(20)]
    public string? InstallationPermitNumber { get; set; }
    [MaxLength(20)]
    public string? EnvironmentalPermitWasteManagementNumber { get; set; }
    [MaxLength(20)]
    public string? ApplicationReferenceNumber { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal PPCReprocessingCapacityTonne { get; set; }
    public decimal WasteManagementReprocessingCapacityTonne { get; set; }
    public decimal InstallationReprocessingTonne { get; set; }
    public decimal EnvironmentalPermitWasteManagementTonne { get; set; }
    [ForeignKey("PPCPeriod")]
    public int? PPCPeriodId { get; set; }
    public LookupPeriod? PPCPeriod { get; set; }
    [ForeignKey("WasteManagementPeriod")]
    public int? WasteManagementPeriodId { get; set; }
    public LookupPeriod? WasteManagementPeriod { get; set; }
    [ForeignKey("InstallationPeriod")]
    public int? InstallationPeriodId { get; set; }
    public LookupPeriod? InstallationPeriod { get; set; }
    [ForeignKey("EnvironmentalPermitWasteManagementPeriod")]
    public int? EnvironmentalPermitWasteManagementPeriodId { get; set; }
    public LookupPeriod? EnvironmentalPermitWasteManagementPeriod { get; set; }
    public decimal MaximumReprocessingCapacityTonne { get; set; }
    [ForeignKey("MaximumReprocessingPeriod")]
    public int? MaximumReprocessingPeriodId { get; set; }
    public DulyMade? DulyMade { get; set; }
    public DeterminationDate? DeterminationDate { get; set; }
    public LookupPeriod? MaximumReprocessingPeriod { get; set; }
    public bool IsMaterialRegistered { get; set; } = false;
    public List<RegistrationReprocessingIO>? RegistrationReprocessingIO { get; set; }
    public List<RegistrationFileUpload>? FileUploads { get; set; }
    public List<MaterialExemptionReference>? MaterialExemptionReferences { get; set; }
    public List<RegulatorApplicationTaskStatus>? Tasks { get; set; }
    public List<ApplicantRegistrationTaskStatus>? ApplicantTaskStatuses { get; set; }
    public List<Accreditation>? Accreditations { get; set; }
}