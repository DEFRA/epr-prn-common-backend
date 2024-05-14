namespace EPR.Accreditation.API.UnitTests.Helpers
{
    using EPR.Accreditation.API.Common.Data.DataModels;
    using EPR.Accreditation.API.Helpers.Comparers;

    [TestClass]
    public class ReprocessorSupportingInformationDataComparerTests
    {
        private ReprocessorSupportingInformationDataComparer _reprocessorSupportingInformationDataComparer;

        [TestInitialize]
        public void Init()
        {
            _reprocessorSupportingInformationDataComparer = new ReprocessorSupportingInformationDataComparer();
        }

        [TestMethod]
        public void SuccessfullyComparesObjects()
        {
            // Arrange
            var a = new ReprocessorSupportingInformation
            {
                Type = "AAA",
                Tonnes = 1.1m
            };

            var b = new ReprocessorSupportingInformation
            {
                Type = "AAA",
                Tonnes = 1.1m
            };

            var c = new ReprocessorSupportingInformation
            {
                Type = "ZZZ",
                Tonnes = 1.1m
            };

            var d = new ReprocessorSupportingInformation
            {
                Type = "AAA",
                Tonnes = 1.0m
            };

            // Act
            var result1 = _reprocessorSupportingInformationDataComparer.Equals(a, b);
            var result2 = _reprocessorSupportingInformationDataComparer.Equals(a, c);
            var result3 = _reprocessorSupportingInformationDataComparer.Equals(a, d);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
            Assert.IsFalse(result3);
        }

        [TestMethod]
        public void GeneratesExpectedHashCode()
        {
            // Arrange
            var a = new ReprocessorSupportingInformation
            {
                Type = "AAA",
                Tonnes = 1.1m
            };

            // Act
            var result = _reprocessorSupportingInformationDataComparer.GetHashCode(a);

            // Assert
            Assert.IsTrue(result == (a.Type.GetHashCode() ^ a.Tonnes.GetHashCode()));
        }
    }
}
