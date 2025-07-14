using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.Data.DTO
{
    public class UpdateOverseasAddressDto
    {
        public List<OverseasAddressDto> OverseasAddresses { get; set; } = [];
        public Guid RegistrationMaterialId { get; set; }
    }
}
