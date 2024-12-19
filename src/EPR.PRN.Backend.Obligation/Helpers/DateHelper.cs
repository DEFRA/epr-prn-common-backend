namespace EPR.PRN.Backend.Obligation.Helpers
{
    public static class DateHelper
    {
        public static int ExtractYear(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                throw new ArgumentException("Input string cannot be null or empty.", nameof(dateString));
            }

            // Split the string by '-'
            var parts = dateString.Split('-');

            // Check if the first part is a valid year
            if (parts.Length > 0 && int.TryParse(parts[0], out int year))
            {
                // Return year plus one as per domain requirements
                return new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc).Year + 1;
            }

            throw new FormatException("The input string is not in the correct format.");
        }
    }
}
