using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.Data.DataModels.Accreditations
{
    [ExcludeFromCodeCoverage]

    [Table("Public.AccreditationTaskStatus")]
    public class AccreditationTaskStatus : TaskStatusBase
    {



        public LookupApplicantRegistrationTask Task { get; set; } = null!;
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public required Accreditation Accreditation { get; set; }
        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }
    }
}
