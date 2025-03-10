using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Name{ get; set; }

    }
}