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
    public string Name { get; set; }
}

public class LookupMaterial : LookupBase
{
    public string MaterialCode { get; set; }
}
public class LookupRegistrationMaterialStatus : LookupBase{}

public class LookupTask : LookupBase
{
    public bool IsMaterialSpecific { get; set; }
}

public class LookupRegistrationStatus : LookupBase{}

public class LookupTaskStatus : LookupBase{}
