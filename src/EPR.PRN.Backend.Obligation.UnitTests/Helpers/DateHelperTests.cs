using EPR.PRN.Backend.Obligation.Helpers;
using FluentAssertions;

namespace EPR.PRN.Backend.Obligation.UnitTests.Helpers;

[TestClass]
public class DateHelperTests
{
    [TestMethod]
    public void ExtractYear_ValidDateString_ShouldReturnYear()
    {
        // Arrange
        string dateString = "2024-P4";

        // Act
        int result = DateHelper.ExtractYear(dateString);

        // Assert
        result.Should().Be(2025);
    }

    [TestMethod]
    public void ExtractYear_EmptyString_ShouldThrowArgumentException()
    {
        // Arrange
        string dateString = "";

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Input string cannot be null or empty. (Parameter 'dateString')");
    }

    [TestMethod]
    public void ExtractYear_NullString_ShouldThrowArgumentException()
    {
        // Arrange
        string dateString = null;

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Input string cannot be null or empty. (Parameter 'dateString')");
    }

    [TestMethod]
    public void ExtractYear_InvalidFormat_ShouldThrowFormatException()
    {
        // Arrange
        string dateString = "InvalidFormat";

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("The input string is not in the correct format.");
    }

    [TestMethod]
    public void ExtractYear_InvalidYear_ShouldThrowFormatException()
    {
        // Arrange
        string dateString = "abcd-P4";

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("The input string is not in the correct format.");
    }

    [TestMethod]
    public void ExtractYear_ValidLeapYear_ShouldReturnYear()
    {
        // Arrange
        string dateString = "2020-P4";

        // Act
        int result = DateHelper.ExtractYear(dateString);

        // Assert
        result.Should().Be(2021);
    }

    [TestMethod]
    public void ExtractYear_StringWithOnlyDash_ShouldThrowFormatException()
    {
        // Arrange
        string dateString = "-";

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("The input string is not in the correct format.");
    }

    [TestMethod]
    public void ExtractYear_StringWithMultipleDashes_ShouldThrowFormatException()
    {
        // Arrange
        string dateString = "--P4";

        // Act
        Action act = () => DateHelper.ExtractYear(dateString);

        // Assert
        act.Should().Throw<FormatException>()
            .WithMessage("The input string is not in the correct format.");
    }
}
