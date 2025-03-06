namespace EPR.PRN.Backend.Data.DataModels
{
    public class TaskStatus: BaseModel
    {
        public virtual ICollection<RegistrationTaskStatus> RegistrationTasks { get; set; } = null!;
    }
}
