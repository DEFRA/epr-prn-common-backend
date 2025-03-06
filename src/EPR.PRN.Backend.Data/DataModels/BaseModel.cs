using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
