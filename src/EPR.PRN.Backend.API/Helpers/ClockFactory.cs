using System.Globalization;

namespace EPR.PRN.Backend.API.Helpers;

/// <summary>
/// Builds the clock the service reads. With <c>StartupUtcTimestampOverride</c> set, the clock starts at that moment
/// and then advances with real time, so date-dependent rules (such as which year a December waste note can be accepted
/// into) can be tried on a local stack. The override is honoured only in the Development environment.
/// </summary>
public static class ClockFactory
{
    public const string OverrideSetting = "StartupUtcTimestampOverride";

    public static TimeProvider Create(IConfiguration configuration, bool isDevelopment, TimeProvider? real = null)
    {
        var realClock = real ?? TimeProvider.System;
        var text = configuration[OverrideSetting];

        if (!isDevelopment || string.IsNullOrWhiteSpace(text))
        {
            return realClock;
        }

        if (!DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var start))
        {
            throw new FormatException($"{OverrideSetting} must be an RFC 3339 timestamp, got '{text}'");
        }

        return new ShiftedTimeProvider(start.ToUniversalTime(), realClock);
    }

    private sealed class ShiftedTimeProvider(DateTimeOffset start, TimeProvider real) : TimeProvider
    {
        private readonly TimeSpan _offset = start - real.GetUtcNow();

        public override DateTimeOffset GetUtcNow() => real.GetUtcNow() + _offset;
    }
}
