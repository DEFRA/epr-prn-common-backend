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
                return new DateTime(year, 1, 1).Year;
            }

            throw new FormatException("The input string is not in the correct format.");
        }
    }
}
