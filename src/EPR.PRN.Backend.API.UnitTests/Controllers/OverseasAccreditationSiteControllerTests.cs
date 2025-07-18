using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using EPR.PRN.Backend.API.Controllers.Accreditation;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class OverseasAccreditationSiteControllerTests
{
    private Mock<IOverseasAccreditationSiteService> _serviceMock;
    private OverseasAccreditationSiteController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IOverseasAccreditationSiteService>();
        _controller = new OverseasAccreditationSiteController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnSiteDtos_WhenItemsExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var siteDtos = new List<OverseasAccreditationSiteDto>
        {
            new OverseasAccreditationSiteDto {
                ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 1, SiteCheckStatusId = 2
            }
        };
        _serviceMock.Setup(s => s.GetAllByAccreditationId(accreditationId)).ReturnsAsync(siteDtos);

        // Act
        var result = await _controller.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var sitesResult = result as OkObjectResult;
        sitesResult.Value.Should().BeEquivalentTo(siteDtos);
        _serviceMock.Verify(s => s.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetAllByAccreditationId_ShouldReturnEmptyList_WhenNoItems()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var siteDtos = new List<OverseasAccreditationSiteDto>();
        _serviceMock.Setup(s => s.GetAllByAccreditationId(accreditationId)).ReturnsAsync(siteDtos);

        // Act
        var result = await _controller.GetAllByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var sitesResult = result as OkObjectResult;
        sitesResult.Value.Should().BeEquivalentTo(siteDtos);
        _serviceMock.Verify(s => s.GetAllByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Post_ShouldReturnNoContent()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new OverseasAccreditationSiteDto {
            ExternalId = accreditationId, OrganisationName = "Hun Manet Recycler Ltd", MeetConditionsOfExportId = 2, SiteCheckStatusId = 2
        };

        // Act
        var result = await _controller.Post(accreditationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.PostByAccreditationId(accreditationId, request), Times.Once);
    }
}
