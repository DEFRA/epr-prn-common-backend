namespace EPR.PRN.Backend.Data.Helpers
{
    public static class StringHelper
    {
        public static string TruncateString(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= maxLength)
                return input;

            return maxLength < 3
                ? string.Empty
                : string.Concat(input.AsSpan(0, maxLength - 3), "...");
        }
    }
}
