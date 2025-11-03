using EPR.PRN.Backend.Obligation.Interfaces;

namespace EPR.PRN.Backend.Obligation.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public int CurrentYear => DateTime.UtcNow.Year;
}