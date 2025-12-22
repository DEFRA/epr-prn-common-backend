using System.ComponentModel.DataAnnotations;
using EPR.PRN.Backend.Data.Helpers;
using FluentAssertions;

namespace EPR.PRN.Backend.Data.UnitTests.Helpers;

[TestClass]
public class StringHelperTests
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
    public void TruncateString_ShouldTruncateCorrectly_ShortLengths()
    {
        StringHelper.TruncateString("hello", 4).Should().Be("h...");
        StringHelper.TruncateString("hello", 3).Should().Be("hel");
        StringHelper.TruncateString("hello", 2).Should().Be("he");
        StringHelper.TruncateString("hello", 1).Should().Be("h");
        StringHelper.TruncateString("hello", 0).Should().Be("");

        StringHelper.TruncateString("hello", -1).Should().Be("hello");
    }

    class TestClass
    {
        [MaxLength(10)]
        public string ShortString { get; set; }

        [MaxLength(10)]
        public string NullString { get; set; }

        [MaxLength(10)]
        private string PrivateString { get; set; } = "SomethingLong";

        public string GetPrivateString() => PrivateString;

        [MaxLength(10)]
        private readonly string _privateString2 = "SomethingLong";

        public string GetPrivateString2() => _privateString2;

        [MaxLength(8)]
        public string LongString { get; set; }

        public string NoMaxLengthString { get; set; }

        [MaxLength(5)]
        public string InitOnlyLongString { get; init; } = "This string is initonly";

        [MaxLength(5)]
        public string ReadonlyString { get; } = "This string is readonly";
    }

    class TestClass2
    {
        [MaxLength(4)]
        public string ShortString1 { get; set; }

        [MaxLength(4)]
        public string ShortString2 { get; set; }

        [MaxLength(4)]
        public string ShortString3 { get; set; }
    }

    [TestMethod]
    public void TruncateStringsBasedOnMaxLengthAttributes_ShouldTruncateProperties()
    {
        var testObj = new TestClass
        {
            ShortString = "Short",
            NullString = null,
            LongString = "This string is definitely too long",
            NoMaxLengthString = "This string has no max length attribute",
            InitOnlyLongString = "This string has no max length attribute",
        };

        testObj.TruncateStringsBasedOnMaxLengthAttributes();

        testObj.ShortString.Should().Be("Short");
        testObj.NullString.Should().BeNull();
        testObj.LongString.Should().Be("This ...");
        testObj.GetPrivateString().Should().Be("SomethingLong");
        testObj.GetPrivateString2().Should().Be("SomethingLong");
        testObj.NoMaxLengthString.Should().Be("This string has no max length attribute");
        testObj.InitOnlyLongString.Should().Be("Th...");
        testObj.ReadonlyString.Should().Be("This string is readonly");
    }

    [TestMethod]
    public void TruncateStringsBasedOnMaxLengthAttributes_ShouldTruncateProperties_WithExcludedProperties()
    {
        var testObj = new TestClass2
        {
            ShortString1 = "abcde",
            ShortString2 = "defgh",
            ShortString3 = "ggggg",
        };

        testObj.TruncateStringsBasedOnMaxLengthAttributes([nameof(TestClass2.ShortString2)]);

        testObj.ShortString1.Should().Be("a...");
        testObj.ShortString2.Should().Be("defgh");
        testObj.ShortString3.Should().Be("g...");
    }
}
