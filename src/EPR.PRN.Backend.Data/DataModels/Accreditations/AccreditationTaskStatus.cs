using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
