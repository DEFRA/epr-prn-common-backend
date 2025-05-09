using EPR.PRN.Backend.API.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public class RegistrationMaterial
{
    [Key]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }

    [ForeignKey("Registration")]
    public int RegistrationId { get; set; }

    public Registration Registration { get; set; }
    
    public LookupMaterial Material { get; set; }

    [ForeignKey("Material")]
    public int MaterialId { get; set; }
    
    public int PermitId { get; set; }
    public LookupRegistrationMaterialStatus? Status { get; set; }
    [ForeignKey("Status")]
    public int? StatusID { get; set; }
    public string? ReferenceNumber { get; set; } = null;
    public string? Comments { get; set; } = string.Empty;        
    public decimal MaximumProcessingCapacityTonnes { get; set; }
    public DateTime DeterminationDate { get; set; }
    public DateTime ProcessingStartDate { get; set; }
    public DateTime ProcessingEndDate { get; set; }
    public DateTime StatusUpdatedDate { get; set; }
    public string? StatusUpdatedBy { get; set; } 
    public string? ReasonforNotreg { get; set; } = string.Empty;
    public string Wastecarrierbrokerdealerregistration { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public List<RegulatorApplicationTaskStatus>? Tasks { get; set; }

    public LookupMaterialPermit? PermitType { get; set; }
    
    [ForeignKey("PermitType")]
    public int PermitTypeId { get; set; }

    public List<MaterialExemptionReference>? MaterialExemptionReferences { get; set; }
    public string? PPCPermitNumber { get; set; }
    public string? WasteManagementLicenceNumber { get; set; }
    public string? InstallationPermitNumber { get; set; }
    public string? EnvironmentalPermitWasteManagementNumber { get; set; }
    public decimal PPCReprocessingCapacityTonne { get; set; }
    public decimal WasteManagementReprocessingCapacityTonne { get; set; }
    public decimal InstallationReprocessingTonne { get; set; }
    public decimal EnvironmentalPermitWasteManagementTonne { get; set; }
    public LookupPeriod? PPCPeriod { get; set; }
    [ForeignKey("PPCPeriod")]
    public int PPCPeriodId { get; set; }
    public LookupPeriod? WasteManagementPeriod { get; set; }
    [ForeignKey("WasteManagementPeriod")]
    public int WasteManagementPeriodId { get; set; }
    public LookupPeriod? InstallationPeriod { get; set; }
    [ForeignKey("InstallationPeriod")]
    public int InstallationPeriodId { get; set; }
    public LookupPeriod? EnvironmentalPermitWasteManagementPeriod { get; set; }
    [ForeignKey("EnvironmentalPermitWasteManagementPeriod")]
    public int EnvironmentalPermitWasteManagementPeriodId { get; set; }
    public decimal MaximumReprocessingCapacityTonne { get; set; }
    public LookupPeriod? MaximumReprocessingPeriod { get; set; }
    [ForeignKey("MaximumReprocessingPeriod")]
    public int MaximumReprocessingPeriodID { get; set; }
    public List<RegistrationReprocessingIO>? RegistrationReprocessingIO { get; set; }
    public List<FileUpload>? FileUploads { get; set; }
    public bool IsMaterialRegistered { get; set; } = false;
}