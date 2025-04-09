using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EPR.PRN.Backend.Data.DataModels.Registrations;

public abstract class LookupBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }
}

public class LookupRegistrationMaterialStatus : LookupBase{}

public class LookupTask : LookupBase{}

public class LookupRegistrationStatus : LookupBase{}

public class LookupTaskStatus : LookupBase { public bool IsMaterialSpecific { get; set; } }

public class LookupApplicationType : LookupBase { }

public class LookupPrincipleType : LookupBase { }

public class LookupMaterialPermit : LookupBase { }

public class LookupPeriod : LookupBase { }