using EPR.PRN.Backend.API.Controllers.Accreditation;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.API.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class AccreditationPrnIssueAuthControllerTests
{
    private Mock<IAccreditationPrnIssueAuthService> _serviceMock;
    private AccreditationPrnIssueAuthController _controller;

    [TestInitialize]
    public void SetUp()
    {
        _serviceMock = new Mock<IAccreditationPrnIssueAuthService>();
        _controller = new AccreditationPrnIssueAuthController(_serviceMock.Object);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnOk_WhenItemsExist()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var dtos = new List<AccreditationPrnIssueAuthDto>
        {
            new AccreditationPrnIssueAuthDto { ExternalId = Guid.NewGuid(), AccreditationExternalId = accreditationId },
            new AccreditationPrnIssueAuthDto { ExternalId = Guid.NewGuid(), AccreditationExternalId = accreditationId }
        };
        _serviceMock.Setup(s => s.GetByAccreditationId(accreditationId)).ReturnsAsync(dtos);

        // Act
        var result = await _controller.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(dtos);
        _serviceMock.Verify(s => s.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task GetByAccreditationId_ShouldReturnOk_WithEmptyList_WhenNoItems()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var dtos = new List<AccreditationPrnIssueAuthDto>();
        _serviceMock.Setup(s => s.GetByAccreditationId(accreditationId)).ReturnsAsync(dtos);

        // Act
        var result = await _controller.GetByAccreditationId(accreditationId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(dtos);
        _serviceMock.Verify(s => s.GetByAccreditationId(accreditationId), Times.Once);
    }

    [TestMethod]
    public async Task ReplaceAllByAccreditationId_ShouldReturnNoContent()
    {
        // Arrange
        var accreditationId = Guid.NewGuid();
        var request = new List<AccreditationPrnIssueAuthRequestDto>
        {
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() },
            new AccreditationPrnIssueAuthRequestDto { PersonExternalId = Guid.NewGuid() }
        };

        // Act
        var result = await _controller.Post(accreditationId, request);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _serviceMock.Verify(s => s.ReplaceAllByAccreditationId(accreditationId, request), Times.Once);
    }
}
