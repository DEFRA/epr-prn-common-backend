using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace EPR.PRN.Backend.Data.DataModels.Registrations;


[ExcludeFromCodeCoverage]
public abstract class LookupBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public virtual required string Name { get; set; }
}

[Table("Lookup.RegistrationMaterialStatus")]
public class LookupRegistrationMaterialStatus : LookupBase{}

[Table("Lookup.RegulatorTask")]
public class LookupRegulatorTask : LookupBase
{
    public bool IsMaterialSpecific { get; set; }

    public int ApplicationTypeId { get; set; }

    public int JourneyTypeId { get; set; }
}

[Table("Lookup.TaskStatus")]
public class LookupTaskStatus : LookupBase {
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.MaterialPermit")]
public class LookupMaterialPermit : LookupBase { }

[Table("Lookup.ApplicationType")]
public class LookupApplicationType : LookupBase { 
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.Period")]
public class LookupPeriod : LookupBase
{
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.JourneyType")]
public class LookupJourneyType : LookupBase
{
    [MaxLength(30)]
    public override required string Name { get; set; }
}

[Table("Lookup.FileUploadType")]
public class LookupFileUploadType : LookupBase { }

[Table("Lookup.FileUploadStatus")]
public class LookupFileUploadStatus : LookupBase { }
