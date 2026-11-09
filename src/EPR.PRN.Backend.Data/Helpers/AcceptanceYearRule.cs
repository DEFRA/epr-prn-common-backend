using System.Globalization;
using System.Linq.Expressions;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.Data.Helpers;

/// <summary>
/// The single definition of which years a PRN/PERN can be accepted into at a given moment.
/// Mirrors the frontends' rule: most notes are acceptable only into the current compliance year;
/// December waste may also, or instead, be accepted into the following year.
/// </summary>
public static class AcceptanceYearRule
{
    private const int DecemberWasteNoChoiceYear = 2025;
    private const int DecemberWasteNoChoiceUntilYear = 2026;
    private const int MinimumSupportedYear = 1;
    private const int MaximumSupportedYear = 9998;
    private static readonly TimeZoneInfo UkZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

    /// <summary>The compliance year runs February to January, judged on the UK calendar.</summary>
    public static int ComplianceYear(DateTimeOffset now)
    {
        var ukNow = TimeZoneInfo.ConvertTime(now, UkZone);

        return ukNow.Month == 1 ? ukNow.Year - 1 : ukNow.Year;
    }

    /// <summary>The years a note with this obligation year may be accepted into right now.</summary>
    public static IReadOnlyList<int> AvailableYears(int obligationYear, bool decemberWaste, DateTimeOffset now)
    {
        // The rule builds dates for the year and the one after it, which a DateTimeOffset cannot hold outside these years.
        if (obligationYear < MinimumSupportedYear || obligationYear > MaximumSupportedYear)
        {
            return [];
        }

        var thisComplianceYear = ComplianceYear(now);

        if (obligationYear > thisComplianceYear)
        {
            return [];
        }

        if (!decemberWaste)
        {
            return obligationYear == thisComplianceYear ? [obligationYear] : [];
        }

        if (obligationYear == DecemberWasteNoChoiceYear && thisComplianceYear <= DecemberWasteNoChoiceUntilYear)
        {
            return [thisComplianceYear];
        }

        var windowStart = new DateTimeOffset(obligationYear, 12, 1, 0, 0, 0, TimeSpan.Zero);
        var windowEnd = new DateTimeOffset(obligationYear + 1, 2, 1, 0, 0, 0, TimeSpan.Zero);

        if (now >= windowStart && now < windowEnd)
        {
            return [obligationYear, obligationYear + 1];
        }

        if (obligationYear == thisComplianceYear)
        {
            return [obligationYear];
        }

        return obligationYear + 1 == thisComplianceYear ? [thisComplianceYear] : [];
    }

    /// <summary>
    /// The obligation years of notes that can be accepted into <paramref name="year"/> now,
    /// so a query can test a column rather than evaluate the rule per row.
    /// </summary>
    public static int[] ObligationYearsFor(int year, bool decemberWaste, DateTimeOffset now)
    {
        // A note is only ever acceptable into its own year or the one after it.
        return new[] { year - 1, year }
            .Where(obligationYear => AvailableYears(obligationYear, decemberWaste, now).Contains(year))
            .ToArray();
    }

    /// <summary>
    /// Query condition for notes that can be accepted into <paramref name="year"/> now. Status is not
    /// part of it: combine it with the status filter the caller needs.
    /// </summary>
    public static Expression<Func<Eprn, bool>> IsAcceptableInto(int year, DateTimeOffset now)
    {
        var standardYears = ObligationYearsAsText(year, false, now);
        var decemberWasteYears = ObligationYearsAsText(year, true, now);

        return prn =>
            (!prn.DecemberWaste && standardYears.Contains(prn.ObligationYear))
            || (prn.DecemberWaste && decemberWasteYears.Contains(prn.ObligationYear));
    }

    private static string[] ObligationYearsAsText(int year, bool decemberWaste, DateTimeOffset now) =>
        ObligationYearsFor(year, decemberWaste, now)
            .Select(obligationYear => obligationYear.ToString(CultureInfo.InvariantCulture))
            .ToArray();
}
