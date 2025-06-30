using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations;

public class AccreditationEntity
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public Guid OrganisationId { get; set; }
    public int RegistrationMaterialId { get; set; }
   
    public int AccreditationStatusId { get; set; }

    [MaxLength(50)]
    public string? DecFullName { get; set; }

    [MaxLength(50)]
    public string? JobTitle { get; set; }

    [MaxLength(18)]
    public string? ApplicationReferenceNumber { get; set; }

    public int? AccreditationYear { get; set; }
    public int? PRNTonnage { get; set; }
    public bool PrnTonnageAndAuthoritiesConfirmed { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? InfrastructurePercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")] 
    public decimal? RecycledWastePercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? BusinessCollectionsPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? NewUsersRecycledPackagingWastePercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? NewMarketsPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? CommunicationsPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? NotCoveredOtherCategoriesPercentage { get; set; }

    [MaxLength(500)]
    public string? InfrastructureNotes { get; set; }

    [MaxLength(500)]
    public string? RecycledWasteNotes { get; set; }

    [MaxLength(500)]
    public string? BusinessCollectionsNotes { get; set; }

    [MaxLength(500)]
    public string? NewUsersRecycledPackagingWasteNotes { get; set; }

    [MaxLength(500)]
    public string? NewMarketsNotes { get; set; }

    [MaxLength(500)]
    public string? CommunicationsNotes { get; set; }

    [MaxLength(500)]
    public string? NotCoveredOtherCategoriesNotes { get; set; }

    public bool BusinessPlanConfirmed { get; set; }

    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }


    public int ApplicationTypeId { get; set; }
    public ApplicationType? ApplicationType { get; set; }

    public AccreditationStatus? AccreditationStatus { get; set; }

    public RegistrationMaterial? RegistrationMaterial { get; set; }

    public List<AccreditationPrnIssueAuth>? AccreditationPrnIssueAuths { get; set; }

    public List<AccreditationFileUpload>? FileUploads { get; set; }
}