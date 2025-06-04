using EPR.PRN.Backend.Obligation.Providers;
using FluentAssertions;

namespace EPR.PRN.Backend.Obligation.UnitTests.Providers;

[TestClass]
public class DateTimeProviderTests
{
	private DateTimeProvider _dateTimeProvider;

	[TestInitialize]
	public void Setup()
	{
		_dateTimeProvider = new DateTimeProvider();
	}

	[TestMethod]
	public void UtcNow_ShouldReturnCurrentUtcTime()
	{
		// Act
		var actualUtcNow = _dateTimeProvider.UtcNow;
		var expectedUtcNow = DateTime.UtcNow;

		// Assert
		actualUtcNow.Should().BeCloseTo(expectedUtcNow, TimeSpan.FromSeconds(1),
			"UtcNow should be very close to the system's current UTC time");
	}

	[TestMethod]
	public void CurrentYear_ShouldReturnCurrentYear()
	{
		// Act
		var actualYear = _dateTimeProvider.CurrentYear;
		var expectedYear = DateTime.UtcNow.Year;

		// Assert
		actualYear.Should().Be(expectedYear, "CurrentYear should match DateTime.UtcNow.Year");
	}
}

