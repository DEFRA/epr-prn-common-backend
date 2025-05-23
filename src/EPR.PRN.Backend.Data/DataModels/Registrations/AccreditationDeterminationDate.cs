using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations;
[Table("Public.AccreditationDeterminationDate")]
[ExcludeFromCodeCoverage]
public class AccreditationDeterminationDate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    [ForeignKey("Accreditation")]
    public int AccreditationId { get; set; }   
    public Accreditation? Accreditation { get; set; }    
    public DateTime? DeterminationDate { get; set; }
  }