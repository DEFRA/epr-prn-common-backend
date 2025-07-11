using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.DTO.Accreditiation;
using EPR.PRN.Backend.Data.Interfaces.Accreditations;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class GetAccreditationsOverviewByOrgIdQueryHandlerTests
    {
        private Mock<IAccreditationRepository> _mockAccreditationRepository;
        private Mock<IValidationService> _mockValidationService;
        private GetAccreditationsOverviewByOrgIdQueryHandler _handlerUnderTest;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAccreditationRepository = new Mock<IAccreditationRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _handlerUnderTest = new GetAccreditationsOverviewByOrgIdQueryHandler(_mockAccreditationRepository.Object, _mockValidationService.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnMappedDto_WhenAccreditationExists()
        {
            // Arrange
            var organisationId = Guid.NewGuid();
            var externalId = Guid.NewGuid();
            var query = new GetAccreditationsOverviewByOrgIdQuery { OrganisationId = organisationId };

            var expectedDto = new List<AccreditationOverviewDto>
            {
                new AccreditationOverviewDto
                {
                    OrganisationId = organisationId
                },
                new AccreditationOverviewDto
                {
                    OrganisationId = organisationId
                },
                new AccreditationOverviewDto
                {
                    OrganisationId = organisationId
                },
                new AccreditationOverviewDto
                {
                    OrganisationId = organisationId
                }
            };

            _mockAccreditationRepository.Setup(x => x.GetAccreditationOverviewForOrgId(organisationId))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _handlerUnderTest.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnEmptyList_NoAccreditationsFound()
        {
            // Arrange
            var organisationId = Guid.NewGuid();
            var externalId = Guid.NewGuid();
            var query = new GetAccreditationsOverviewByOrgIdQuery { OrganisationId = organisationId };

            var expectedDto = new List<AccreditationOverviewDto>
            {
            };

            _mockAccreditationRepository.Setup(x => x.GetAccreditationOverviewForOrgId(organisationId))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _handlerUnderTest.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
            result.Count().Should().Be(0);
        }
    }
}
