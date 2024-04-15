using EPR.Accreditation.API.Common.Data.DataModels;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Accreditation.API.Helpers.Comparers
{
    public class ReprocessorSupportingInformationDataComparer : IEqualityComparer<ReprocessorSupportingInformation>
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
