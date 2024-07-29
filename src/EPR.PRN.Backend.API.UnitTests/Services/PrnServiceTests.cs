using AutoFixture.MSTest;
using EPR.PRN.Backend.API.Common.DTO;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PrnService _systemUnderTest;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _systemUnderTest = new PrnService(_mockRepository.Object);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrnForOrganisationById_WithValidId_ReturnsExpectedDto(Guid prnId, Guid orgId, EPRN expectedPrn)
        {
            _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync(expectedPrn);

            var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

            result.Should().BeEquivalentTo((PrnDTo)expectedPrn);
        }

        [TestMethod]
        [AutoData]
        public async Task GetPrnForOrganisationById_WithInValidId_ReturnsNull(Guid prnId, Guid orgId, EPRN expectedPrn)
        {
            _mockRepository.Setup(r => r.GetPrnForOrganisationById(orgId, prnId)).ReturnsAsync((EPRN?)null);

            var result = await _systemUnderTest.GetPrnForOrganisationById(orgId, prnId);

            result.Should().BeNull();
        }

        [TestMethod]
        [AutoData]
        public async Task GetAllPrnByOrganisationId_WithInValidId_ReturnsNull(Guid orgId, List<EPRN> expectedPrns)
        {
            _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

            var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId);

            result.Should().BeEquivalentTo(expectedPrns);
        }

        [TestMethod]
        [AutoData]
        public async Task UpdateStatus_Return_ReturnsNull(Guid orgId, List<EPRN> expectedPrns)
        {
            _mockRepository.Setup(r => r.GetAllPrnByOrganisationId(orgId)).ReturnsAsync(expectedPrns);

            var result = await _systemUnderTest.GetAllPrnByOrganisationId(orgId);

            result.Should().BeEquivalentTo(expectedPrns);
        }
    }
}
