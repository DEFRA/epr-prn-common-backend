using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RecyclingTarget
    {
        [Key]
        public int Id { get; set; }

        public int Year { get; set; }

        public required string MaterialNameRT { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double Target { get; set; }
    }
}