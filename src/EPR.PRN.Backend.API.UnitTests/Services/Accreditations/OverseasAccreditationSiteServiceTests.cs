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
        var request = new OverseasAccreditationSiteDto
        {
            ExternalId = accreditationId,
            OrganisationName = "Hun Manet Recycler Ltd",
            MeetConditionsOfExportId = 2,
            SiteCheckStatusId = 2
        };

        // Act
        await _service.PostByAccreditationId(accreditationId, request);

        // Assert
        _repositoryMock.Verify(r => r.PostByAccreditationId(accreditationId, It.IsAny<OverseasAccreditationSite>()), Times.Once);
    }

    [TestMethod]
    public void OverseasAccreditationSite_AllProperties_AreAccessible()
    {
        var entity = new OverseasAccreditationSite
        {
            Id = 1,
            ExternalId = Guid.NewGuid(),
            AccreditationId = 123,
            OverseasAddressId = 456,
            OrganisationName = "Test Org",
            MeetConditionsOfExportId = 3,
            StartDay = 1,
            StartMonth = 2,
            StartYear = 2024,
            ExpiryDay = 30,
            ExpiryMonth = 12,
            ExpiryYear = 2025,
            CreatedBy = Guid.NewGuid(),
            CreatedOn = DateTime.UtcNow,
            SiteCheckStatusId = 4
        };

        // Touch every property
        Assert.AreEqual(1, entity.Id);
        Assert.IsNotNull(entity.ExternalId);
        Assert.AreEqual("Test Org", entity.OrganisationName);
        Assert.AreEqual(2024, entity.StartYear);
        Assert.AreEqual(2025, entity.ExpiryYear);
        Assert.AreEqual(4, entity.SiteCheckStatusId);
        Assert.IsTrue(entity.CreatedOn <= DateTime.UtcNow);
    }
}
