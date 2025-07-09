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
[ExcludeFromCodeCoverage]
public class LookupRegistrationMaterialStatus : LookupBase{}

[Table("Lookup.AccreditationStatus")]
[ExcludeFromCodeCoverage]
public class LookupAccreditationStatus : LookupBase
{
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.RegulatorTask")]
[ExcludeFromCodeCoverage]
public class LookupRegulatorTask : LookupBase
{
    public bool IsMaterialSpecific { get; set; }

    public int ApplicationTypeId { get; set; }

    public int JourneyTypeId { get; set; }
}

[Table("Lookup.Task")]
[ExcludeFromCodeCoverage]
public class LookupApplicantRegistrationTask : LookupBase
{
    public bool IsMaterialSpecific { get; set; }

    public int ApplicationTypeId { get; set; }

    public int JourneyTypeId { get; set; }
}

[Table("Lookup.TaskStatus")]
[ExcludeFromCodeCoverage]
public class LookupTaskStatus : LookupBase {
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.MaterialPermit")]
[ExcludeFromCodeCoverage]
public class LookupMaterialPermit : LookupBase { }

[Table("Lookup.ApplicationType")]
[ExcludeFromCodeCoverage]
public class LookupApplicationType : LookupBase { 
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.Period")]
[ExcludeFromCodeCoverage]
public class LookupPeriod : LookupBase
{
    [MaxLength(100)]
    public override required string Name { get; set; }
}

[Table("Lookup.JourneyType")]
[ExcludeFromCodeCoverage]
public class LookupJourneyType : LookupBase
{
    [MaxLength(30)]
    public override required string Name { get; set; }
}

[Table("Lookup.FileUploadType")]
[ExcludeFromCodeCoverage]
public class LookupFileUploadType : LookupBase { }

[Table("Lookup.FileUploadStatus")]
[ExcludeFromCodeCoverage]
public class LookupFileUploadStatus : LookupBase { }

[Table("Lookup.Country")]
[ExcludeFromCodeCoverage]
public class LookupCountry : LookupBase
{
    [MaxLength(100)]
    public override required string Name { get; set; }
    [MaxLength(3)]
    public string? CountryCode { get; set; }
}
