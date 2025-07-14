using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DTO
{
    public class OverseasAddressWasteCodeDto
    {
        public Guid ExternalId { get; set; }
        public required string CodeName { get; set; }
    }
}
