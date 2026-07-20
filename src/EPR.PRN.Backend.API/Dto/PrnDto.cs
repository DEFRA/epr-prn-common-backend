using System.Diagnostics.CodeAnalysis;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Dto
{
    [ExcludeFromCodeCoverage]
    public class PrnDto : PrnBaseDto
    {
        public EprnStatus PrnStatus => (EprnStatus)PrnStatusId;

        public static implicit operator PrnDto(Eprn prn)
        {
            return PopulateFromEprn(prn, new PrnDto());
        }
    }
}
