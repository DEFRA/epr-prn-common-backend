using EPR.Accreditation.API.Common.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Accreditation.API.Helpers.Comparers
{
    public class ReprocessorSupportingInformationDtoComparer : IEqualityComparer<ReprocessorSupportingInformation>
    {
        public bool Equals(ReprocessorSupportingInformation x, ReprocessorSupportingInformation y)
        {
            return x.Type == y.Type &&
                x.Tonnes == y.Tonnes;
        }

        public int GetHashCode([DisallowNull] ReprocessorSupportingInformation obj)
        {
            return obj.Type.GetHashCode() ^ obj.Tonnes.GetHashCode();
        }
    }
}
