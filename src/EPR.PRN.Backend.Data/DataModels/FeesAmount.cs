using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class FeesAmount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MaterialId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }
    }
}