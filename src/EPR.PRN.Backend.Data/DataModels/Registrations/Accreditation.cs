using EPR.PRN.Backend.Data.DataModels.Accreditations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.Accreditation")]
[ExcludeFromCodeCoverage]
public class Accreditation
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }

    [ForeignKey("ApplicationType")]
    public int ApplicationTypeId { get; set; }
    public LookupApplicationType? ApplicationType { get; set; }

    public Guid OrganisationId { get; set; }

    [ForeignKey("AccreditationStatus")]
    public int AccreditationStatusId { get; set; }
    public LookupAccreditationStatus AccreditationStatus { get; set; }

    [ForeignKey("RegistrationMaterial")]
    public int RegistrationMaterialId { get; set; }
    public RegistrationMaterial RegistrationMaterial { get; set; }

    [MaxLength(50)]
    public string? DecFullName { get; set; }

    [MaxLength(50)]
    public string? JobTitle { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid UpdatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }

    [MaxLength(12)]
    public required string ApplicationReferenceNumber { get; set; }

    public int PRNTonnage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? InfrastructurePercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? BusinessCollectionsPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? RecycledWastePercentage { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal? NewMarketsPercentage { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal? CommunicationsPercentage { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? NewUsersRecycledPackagingWastePercentage { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal? NotCoveredOtherCategoriesPercentage { get; set; }
    [Column(TypeName = "decimal(10,2)")]
    public decimal? TotalPercentage { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string? InfrastructureNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? BusinessCollectionsNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? RecycledWasteNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? NewMarketsNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? CommunicationsNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? NewUsersRecycledPackagingWasteNotes { get; set; }
    [Column(TypeName = "varchar(500)")]
    public string? NotCoveredOtherCategoriesNotes { get; set; }

    public int AccreditationYear { get; set; }
   
    public List<RegulatorAccreditationTaskStatus>? Tasks { get; set; }

    public List<AccreditationDeterminationDate> AccreditationDulyMade { get; set; }        
   
    public List<AccreditationFileUpload>? FileUploads { get; set; }

    public List<AccreditationPrnIssueAuth>? AccreditationPrnIssueAuths { get; set; }

}
