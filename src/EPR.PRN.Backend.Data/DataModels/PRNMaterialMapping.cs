using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class PrnMaterialMapping
    {
        [Key]
        public int Id { get; set; }

        public required int PRNMaterialId { get; set; }

        [MaxLength(50)]
        public required string NPWDMaterialName { get; set; }
    }
}
