using AwesomeAssertions;
using EPR.PRN.Backend.API.Helpers;
using Microsoft.Extensions.Configuration;

namespace EPR.PRN.Backend.API.UnitTests.Helpers;

[TestClass]
public class ClockFactoryTests
{
    private const string Override = "2026-12-15T12:00:00Z";
    private static readonly DateTimeOffset OverrideAt = new(2026, 12, 15, 12, 0, 0, TimeSpan.Zero);

    private static IConfiguration ConfigWith(string? value) =>
        new ConfigurationBuilder()
            .AddInMemoryCollection(value is null ? [] : [new("StartupUtcTimestampOverride", value)])
            .Build();

    [TestMethod]
    public void WithoutAnOverride_UsesTheRealClock()
    {
        var real = new ManualTimeProvider(new DateTimeOffset(2026, 10, 27, 9, 0, 0, TimeSpan.Zero));

        var clock = ClockFactory.Create(ConfigWith(null), isDevelopment: true, real);

        clock.GetUtcNow().Should().Be(real.GetUtcNow());
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void WithABlankOverride_UsesTheRealClock(string blank)
    {
        var real = new ManualTimeProvider(new DateTimeOffset(2026, 10, 27, 9, 0, 0, TimeSpan.Zero));

        var clock = ClockFactory.Create(ConfigWith(blank), isDevelopment: true, real);

        clock.GetUtcNow().Should().Be(real.GetUtcNow());
    }

    [TestMethod]
    public void InDevelopment_StartsAtTheOverrideAndAdvancesWithTheRealClock()
    {
        var real = new ManualTimeProvider(new DateTimeOffset(2026, 10, 27, 9, 0, 0, TimeSpan.Zero));
        var clock = ClockFactory.Create(ConfigWith(Override), isDevelopment: true, real);

        clock.GetUtcNow().Should().Be(OverrideAt);

        real.Advance(TimeSpan.FromMinutes(90));

        clock.GetUtcNow().Should().Be(OverrideAt.AddMinutes(90));
    }

    [TestMethod]
    public void OutsideDevelopment_IgnoresTheOverride()
    {
        var real = new ManualTimeProvider(new DateTimeOffset(2026, 10, 27, 9, 0, 0, TimeSpan.Zero));

        var clock = ClockFactory.Create(ConfigWith(Override), isDevelopment: false, real);

        clock.GetUtcNow().Should().Be(real.GetUtcNow());
    }

    [TestMethod]
    public void WithAnInvalidOverride_FailsAtStartupNamingTheSetting()
    {
        var act = () => ClockFactory.Create(ConfigWith("next tuesday"), isDevelopment: true, TimeProvider.System);

        act.Should().Throw<FormatException>().WithMessage("*StartupUtcTimestampOverride*next tuesday*");
    }

    [TestMethod]
    public void WithAnOverrideOffset_ReadsItAsTheSameInstantInUtc()
    {
        var real = new ManualTimeProvider(new DateTimeOffset(2026, 10, 27, 9, 0, 0, TimeSpan.Zero));

        var clock = ClockFactory.Create(ConfigWith("2026-12-15T13:00:00+01:00"), isDevelopment: true, real);

        clock.GetUtcNow().Should().Be(OverrideAt);
    }

    private sealed class ManualTimeProvider(DateTimeOffset start) : TimeProvider
    {
        private DateTimeOffset _now = start;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }
}
