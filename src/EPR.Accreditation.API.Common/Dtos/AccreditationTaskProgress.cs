namespace EPR.Accreditation.API.Common.Dtos
{
    public class AccreditationTaskProgress
    {
        public Enums.TaskStatus TaskStatusId { get; set; }
        
        public Enums.TaskName TaskNameId { get; set; }
    }
}