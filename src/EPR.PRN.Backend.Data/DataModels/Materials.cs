using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Materials
    {
        [MaxLength(20)]
        public string MaterialName { get; set; }

        [MaxLength(3)]
        public string MaterialCode { get; set; }
    }
}
