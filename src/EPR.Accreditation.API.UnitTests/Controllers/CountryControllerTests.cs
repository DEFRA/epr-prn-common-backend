namespace EPR.Accreditation.API.UnitTests.Controllers
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Controllers;
    using EPR.Accreditation.API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    [TestClass]
    public class CountryControllerTests
    {
        private CountryController _countryController;
        private Mock<ICountryService> _mockCountryService;

        [TestInitialize]
        public void Init()
        {
            _mockCountryService = new Mock<ICountryService>();
            _countryController = new CountryController(_mockCountryService.Object);
        }

        [TestMethod]
        public async Task GetCountries_ReturnsOkResultWithCountries()
        {
            // Arrange
            var expectedCountries = new List<Country>
            {
                new Country { CountryId = 1, Name = "Country 1" },
                new Country { CountryId = 2, Name = "Country 2" }
            };

            _mockCountryService.Setup(s => s.GetCountryList()).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryController.GetCountries() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            CollectionAssert.AreEqual(expectedCountries, (System.Collections.ICollection)(result.Value as IEnumerable<Country>));

            _mockCountryService.Verify(s => s.GetCountryList(), Times.Once);
        }
    }
}
