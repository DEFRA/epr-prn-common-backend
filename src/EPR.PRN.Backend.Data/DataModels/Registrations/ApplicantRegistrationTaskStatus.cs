using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;

[Table("Public.RegistrationTaskStatus")]
[ExcludeFromCodeCoverage]
public class ApplicantRegistrationTaskStatus 
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ExternalId { get; set; }
    
    [ForeignKey("Task")]
    public int? TaskId { get; set; }
    public LookupApplicantRegistrationTask Task { get; set; } = null!;

    [ForeignKey("TaskStatus")]
    public int? TaskStatusId { get; set; }
    public LookupTaskStatus TaskStatus { get; set; } = null!;

    public int? RegistrationId { get; set; }
    public Registration Registration { get; set; } = null!;

    public int? RegistrationMaterialId { get; set; }
    public RegistrationMaterial RegistrationMaterial { get; set; } = null!;
}