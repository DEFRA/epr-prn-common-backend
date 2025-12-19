using EPR.PRN.Backend.Data.Helpers;
using FluentAssertions;

namespace EPR.PRN.Backend.Data.UnitTests.Helpers;

[TestClass]
public class StringHelpersTests
{
    [TestMethod]
    [DataRow("", "")]
    [DataRow(null, null)]
    [DataRow("0123456789", "0123456789")]
    [DataRow("01234567890", "0123456...")]
    [DataRow("ThisStringIsWayTooLong", "ThisStr...")]
    public void TruncateString_ShouldTruncateCorrectly(string input, string expected)
    {
        int maxLength = 10;
        StringHelper.TruncateString(input, maxLength).Should().Be(expected);
    }

    [TestMethod]
    public void TruncateString_ShouldTruncateCorrectly_WrongLengths()
    {
        StringHelper.TruncateString("hello", 3).Should().Be("...");
        StringHelper.TruncateString("hello", 2).Should().Be("");
        StringHelper.TruncateString("hello", 1).Should().Be("");
        StringHelper.TruncateString("hello", 0).Should().Be("");

        StringHelper.TruncateString("hello", -1).Should().Be("");
    }
}
