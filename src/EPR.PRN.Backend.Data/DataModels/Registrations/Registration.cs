using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.Registration")]
[ExcludeFromCodeCoverage]
public class Registration
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }
    public int ApplicationTypeId { get; set; }
    public Guid OrganisationId { get; set; }
    public int RegistrationStatusId { get; set; }

    [ForeignKey("BusinessAddress")]
    public int? BusinessAddressId { get; set; }
    
    public Address? BusinessAddress { get; set; }

    [ForeignKey("ReprocessingSiteAddress")]
    public int? ReprocessingSiteAddressId { get; set; }

    public Address? ReprocessingSiteAddress { get; set; }

    // Identifier for the legal document address
    [ForeignKey("LegalDocumentAddress")]
    public int? LegalDocumentAddressId { get; set; }

    public Address? LegalDocumentAddress { get; set; }
    
    public int AssignedOfficerId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public List<RegulatorRegistrationTaskStatus>? Tasks { get; set; }

    public List<RegulatorAccreditationRegistrationTaskStatus>? AccreditationTasks { get; set; }

    public List<RegistrationMaterial>? Materials { get; set; }
}