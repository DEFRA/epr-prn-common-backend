using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class PEprNpwdSync
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [ForeignKey("Prn")]
        public int PRNId { get; set; } // Foreign Key to PRN table

        public int PRNStatusId { get; set; } // PRN Status Id from PRN table

        public DateTime CreatedOn { get; set; } // Creation timestamp
    }
}
