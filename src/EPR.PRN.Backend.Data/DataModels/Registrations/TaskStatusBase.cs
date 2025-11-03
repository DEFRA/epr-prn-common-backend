using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    public class TaskStatusBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }

        public LookupTaskStatus TaskStatus { get; set; } = null!;
        [ForeignKey("TaskStatus")]
        public int TaskStatusId { get; set; }
    }
}
