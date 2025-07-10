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
    /*
 * 
 * Task Name :

PRNs tonnage and authority to issue PRNs

Business Plan

Accreditation sampling and inspection plan

Overseas reprocessing sites and broadly equivalent evidence

Task Status :

Not started

Started

Completed

Cannot start yet
 * 
 * 
 */

    [ExcludeFromCodeCoverage]

    [Table("Public.AccreditationTaskStatus")]
    public class AccreditationTaskStatus : TaskStatusBase
    {
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // [Key]
       // public int Id { get; set; }
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // public Guid ExternalId { get; set; }

       //// [ForeignKey("AccreditationTask")]
       // public LookupTaskStatus Task { get; set; } = null!;

       // public int TaskId { get; set; }

       // //[ForeignKey("AccreditationTaskStatus")]
       // public LookupTaskStatus TaskStatus { get; set; } = null!;
        
       // public int TaskStatusId { get; set; }


        public LookupApplicantRegistrationTask Task { get; set; } = null!;
        [ForeignKey("Task")]
        public int TaskId { get; set; }

        public Accreditation Accreditation { get; set; }
        [ForeignKey("Accreditation")]
        public int AccreditationId { get; set; }
    }
}
