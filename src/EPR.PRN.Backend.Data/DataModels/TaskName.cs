namespace EPR.PRN.Backend.Data.DataModels
{
    public class TaskName: BaseModel
    {
        public virtual ICollection<RegistrationTaskStatus> RegistrationTasks { get; set; } = null!;
    }
}
