namespace EPR.Accreditation.API.UnitTests.Services
{
    using EPR.Accreditation.API.Common.Dtos;
    using EPR.Accreditation.API.Repositories.Interfaces;
    using EPR.Accreditation.API.Services;
    using Moq;

    [TestClass]
    public class CountryServiceTests
    {
        private CountryService _countryService;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _countryService = new CountryService(_mockRepository.Object);
        }

        [TestMethod]
        public async Task GetCountryList_ReturnsListOfCountries()
        {
            // Arrange
            var expectedCountries = new List<Country>
            {
                new Country { CountryId = 1, Name = "Country 1" },
                new Country { CountryId = 2, Name = "Country 2" }
            };

            _mockRepository.Setup(r => r.GetCountries()).ReturnsAsync(expectedCountries);

            // Act
            var result = await _countryService.GetCountryList();

            // Assert
            CollectionAssert.AreEqual(expectedCountries, (System.Collections.ICollection)result);

            _mockRepository.Verify(r => r.GetCountries(), Times.Once());
        }
    }
}
