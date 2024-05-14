namespace EPR.Accreditation.API.UnitTests.Helpers
{
    using EPR.Accreditation.API.Common.Data.DataModels;
    using EPR.Accreditation.API.Helpers.Comparers;

    [TestClass]
    public class WasteCodeDataComparerTests
    {
        private WasteCodeDataComparer _wasteCodeDataComparer;

        [TestInitialize]
        public void Init()
        {
            _wasteCodeDataComparer = new WasteCodeDataComparer();
        }

        [TestMethod]
        public void SuccessfullyComparesObjects()
        {
            // Arrange
            var a = new WasteCode
            {
                Code = "AAA"
            };

            var b = new WasteCode
            {
                Code = "AAA"
            };

            var c = new WasteCode
            {
                Code = "ZZZ"
            };

            // Act
            var result1 = _wasteCodeDataComparer.Equals(a, b);
            var result2 = _wasteCodeDataComparer.Equals(a, c);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void GeneratesExpectedHashCode()
        {
            // Arrange
            var a = new WasteCode
            {
                WasteCodeTypeId = Common.Data.Enums.WasteCodeType.MaterialCommodityCode,
                Code = "AAA"
            };

            // Act
            var result = _wasteCodeDataComparer.GetHashCode(a);

            // Assert
            Assert.IsTrue(result == (a.WasteCodeTypeId.GetHashCode() ^ a.Code.GetHashCode()));
        }
    }
}
