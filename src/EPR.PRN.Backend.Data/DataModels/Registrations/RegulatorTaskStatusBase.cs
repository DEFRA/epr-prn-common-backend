using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    public class RegulatorTaskStatusBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalId { get; set; }
        public LookupRegulatorTask Task { get; set; } = null!;
        [ForeignKey("Task")]
        public int RegulatorTaskId { get; set; }
        public LookupTaskStatus TaskStatus { get; set; } = null!;
        [ForeignKey("TaskStatus")]
        public int TaskStatusId { get; set; }
        public Guid StatusCreatedBy { get; set; }
        public DateTime StatusCreatedDate { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public DateTime? StatusUpdatedDate { get; set; }
    }
}