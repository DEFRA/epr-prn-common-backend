using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class AccreditationControllerTests
{
    private Mock<IAccreditationService> _serviceMock;
    private AccreditationController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IAccreditationService>();
        _controller = new AccreditationController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task GetOrCreateAccreditation_ShouldReturnOk()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var organisationId = Guid.NewGuid();
        var materialId = 2;
        var applicationTypeId = 1;
        var accreditation = new AccreditationDto { ExternalId = accreditationId };

        _serviceMock.Setup(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId))
            .ReturnsAsync(accreditationId);

        // Act
        var result = await _controller.Get(organisationId, materialId, applicationTypeId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditationId);
        _serviceMock.Verify(s => s.GetOrCreateAccreditation(organisationId, materialId, applicationTypeId), Times.Once);
    }

    [TestMethod]
    public async Task Get_ShouldReturnOk_WhenAccreditationExists()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var accreditation = new AccreditationDto { ExternalId = accreditationId };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task Get_ShouldReturnNotFound_WhenAccreditationDoesNotExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync((AccreditationDto)null);

        // Act
        var result = await _controller.Get(accreditationId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _serviceMock.Verify(s => s.GetAccreditationById(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task PostWithNewAccreditation_ShouldReturnOk_WithCreatedAccreditation()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new AccreditationRequestDto { OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.CreateAccreditation(request))
            .ReturnsAsync(accreditationId);

        var accreditation = new AccreditationDto { ExternalId = accreditationId, AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Post(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.CreateAccreditation(request), Times.Once);
    }

    [TestMethod]
    public async Task PostWithExistingAccreditation_ShouldReturnOk_WithUpdatedAccreditation()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new AccreditationRequestDto { ExternalId = accreditationId, OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };

        var accreditation = new AccreditationDto { ExternalId = accreditationId, OrganisationId = Guid.NewGuid(), AccreditationYear = 2026 };
        _serviceMock.Setup(s => s.GetAccreditationById(accreditationId))
            .ReturnsAsync(accreditation);

        // Act
        var result = await _controller.Post(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().Be(accreditation);
        _serviceMock.Verify(s => s.UpdateAccreditation(request), Times.Once);
    }
}