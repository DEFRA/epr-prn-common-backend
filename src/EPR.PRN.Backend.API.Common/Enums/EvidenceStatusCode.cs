
namespace EPR.PRN.Backend.API.Common.Enums
{
    public enum EvidenceStatusCode
    {
        EV_ACCEP,
        EV_ACANCEL,
        EV_CANCEL,
        EV_AWACCEP,
        EV_AWACCEP_EPR
    }

    public static class EvidenceStatusCodeExtensions
    {
        private static readonly Dictionary<EvidenceStatusCode, string> HyphenatedValues = new()
        {
            { EvidenceStatusCode.EV_ACCEP, "EV-ACCEP" },
            { EvidenceStatusCode.EV_ACANCEL, "EV-ACANCEL" },
            { EvidenceStatusCode.EV_CANCEL, "EV-CANCEL" },
            { EvidenceStatusCode.EV_AWACCEP, "EV-AWACCEP" },
            { EvidenceStatusCode.EV_AWACCEP_EPR, "EV-AWACCEP-EPR" }
        };

        public static string ToHyphenatedString(this EvidenceStatusCode statusCode)
        {
            return HyphenatedValues[statusCode];
        }
    }
}
