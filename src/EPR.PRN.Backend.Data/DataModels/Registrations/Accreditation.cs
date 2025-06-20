﻿using System.ComponentModel.DataAnnotations;
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

    [ForeignKey("RegistrationMaterial")]
    public int RegistrationMaterialId { get; set; }
    public RegistrationMaterial RegistrationMaterial { get; set; }
    public int AccreditationYear { get; set; }
    [MaxLength(12)]
    public required string ApplicationReferenceNumber { get; set; }
    public List<RegulatorAccreditationTaskStatus>? Tasks { get; set; }

    [ForeignKey("AccreditationStatus")]
    public int AccreditationStatusId { get; set; }
    public LookupAccreditationStatus AccreditationStatus { get;  set; }
    public List<AccreditationDeterminationDate> AccreditationDulyMade { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int PRNTonnage { get; set; }
    public List<AccreditationFileUpload>? FileUploads { get; set; }
}
