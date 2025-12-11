using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class PrnStatusSync
    {
        public string StatusName { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; }
        public string OrganisationName { get; set; } = string.Empty;
        public string PrnNumber { get; set; } = string.Empty;
        public string? SourceSystemId { get; set; }
    }
}