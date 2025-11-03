using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    [ExcludeFromCodeCoverage]
    public class RegulatorTaskStatusBase : TaskStatusBase
    {

        public LookupRegulatorTask Task { get; set; } = null!;
        [ForeignKey("Task")]
        public int RegulatorTaskId { get; set; }
        public Guid StatusCreatedBy { get; set; }
        public DateTime StatusCreatedDate { get; set; }
        public Guid? StatusUpdatedBy { get; set; }
        public DateTime? StatusUpdatedDate { get; set; }
    }
}