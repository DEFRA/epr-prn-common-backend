namespace EPR.Accreditation.API.UnitTests.Helpers
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Helpers.Comparers;

    [TestClass]
    public class WasteCodeDtoComparerTests
    {
        private WasteCodeDtoComparer _wasteCodeDtoComparer;

        [TestInitialize]
        public void Init()
        {
            _wasteCodeDtoComparer = new WasteCodeDtoComparer();
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
            var result1 = _wasteCodeDtoComparer.Equals(a, b);
            var result2 = _wasteCodeDtoComparer.Equals(a, c);

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
                WasteCodeTypeId = Common.Enums.WasteCodeType.WasteDescriptionCode,
                Code = "AAA"
            };

            // Act
            var result = _wasteCodeDtoComparer.GetHashCode(a);

            // Assert
            Assert.IsTrue(result == (a.WasteCodeTypeId.GetHashCode() ^ a.Code.GetHashCode()));
        }
    }
}
