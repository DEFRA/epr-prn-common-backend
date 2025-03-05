using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Obligation.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class EnumHelper
    {
        public static TEnum? ConvertStringToEnum<TEnum>(string? input) where TEnum : struct, Enum
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            if (Enum.TryParse<TEnum>(input, true, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
