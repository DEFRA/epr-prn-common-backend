using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class RecyclingTarget
    {
        [Key]
        public int Year { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double PaperTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double GlassTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double AluminiumTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double SteelTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double PlasticTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double WoodTarget { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public double GlassRemeltTarget { get; set; }
    }
}