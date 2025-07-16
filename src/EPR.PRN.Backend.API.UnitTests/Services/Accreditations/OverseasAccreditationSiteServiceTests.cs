using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Accreditation;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services.Accreditations;

[TestClass]
public class OverseasAccreditationSiteServiceTests
{
    private Mock<IOverseasAccreditationSiteRepository> _repositoryMock;
    private Mock<ILogger<OverseasAccreditationSiteService>> _loggerMock;
    private OverseasAccreditationSiteService _service;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IOverseasAccreditationSiteRepository>();
        _loggerMock = new Mock<ILogger<OverseasAccreditationSiteService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AccreditationProfile>();
        });

        _service = new OverseasAccreditationSiteService(
            _repositoryMock.Object,
            config.CreateMapper(),
            _loggerMock.Object
        );
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ReturnsSiteDtos_WhenEntitiesExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var entities = new List<OverseasAccreditationSite>
        {
            new OverseasAccreditationSite {
                ExternalId = accreditationId, AccreditationId = 1, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 1, SiteCheckStatusId = 2
            }
        };
        var siteDtos = new List<OverseasAccreditationSiteDto>
        {
            new OverseasAccreditationSiteDto {
                ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 1, SiteCheckStatusId = 2
            }
        };
        _repositoryMock.Setup(r => r.GetAllByAccreditationId(accreditationId)).ReturnsAsync(entities);

        // Act
        var result = await _service.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(siteDtos);
        _repositoryMock.Verify(r => r.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ReturnsEmptyList_WhenNoEntities()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var entities = new List<OverseasAccreditationSite>();
        _repositoryMock.Setup(r => r.GetAllByAccreditationId(accreditationId)).ReturnsAsync(entities);

        // Act
        var result = await _service.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _repositoryMock.Verify(r => r.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task PostByAccreditationId_CallsRepositoryWithCorrectParameters()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new OverseasAccreditationSiteDto {
            ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 2, SiteCheckStatusId = 2
        };

        // Act
        await _service.PostByAccreditationId(accreditationId, request);

        // Assert
        _repositoryMock.Verify(r => r.PostByAccreditationId(accreditationId, It.IsAny<OverseasAccreditationSite>()), Times.Once);
    }
}
