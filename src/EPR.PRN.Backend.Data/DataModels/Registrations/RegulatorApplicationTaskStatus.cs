using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels.Registrations
{
    public class RegulatorApplicationTaskStatus
    {
        [Key]
        public int Id { get; set; } // Unique identifier for each record
        public string? ExternalId { get; set; } // External reference identifier
        public int? TaskId { get; set; } // Identifier for the specific task
        public int? TaskStatusId { get; set; } // Identifier for the status of the task
        public string? Comments { get; set; } // Field for storing comments (max length 200)
        public Guid? StatusCreatedBy { get; set; } // Identifier for the user who created the status
        public DateTime? StatusCreatedDate { get; set; } // Date and time when the status was created
        public Guid? StatusUpdatedBy { get; set; } // Identifier for the user who updated the status
        public DateTime? StatusUpdatedDate { get; set; } // Date and time when the status was updated
        public int? RegistrationMaterialId { get; set; } // Identifier for the registration material associated with the task
    }
}