using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Queries;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class MaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<MaterialController>> _loggerMock;
    private MaterialController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<MaterialController>>();

        _controller =
            new MaterialController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAllMaterials_EnsureDataReturnedCorrectly()
    {
        // Arrange
        var materials = new List<MaterialDto>
        {
            new() { Code = "1", Name = "Wood" },
            new() { Code = "2", Name = "Plastic" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetAllMaterialsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(materials);

        // Act
        var result = await _controller.GetAllMaterials();

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(materials);
        }
    }

    [TestMethod]
    public async Task GetMaterials_FilteredBy_RegistrationId_EnsureDataReturnedCorrectly()
    {
        // Arrange
        var materials = new List<MaterialDto>
        {
            new() { Code = "1", Name = "Wood" },
            new() { Code = "2", Name = "Plastic" }
        };

        Guid registrationId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialsByRegistrationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(materials);

        // Act
        var result = await _controller.GetAllMaterials(registrationId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(materials);
        }
    }

    [TestMethod]
    public void ThrowInvalidRegistrationId_ReturnsBadRequest_WhenCalledWithInvalidGuid()
    {
        // Arrange
        var fakeGuid = Guid.NewGuid();

        // Act
        var result = _controller.ThrowInvalidRegistrationId(_loggerMock.Object, fakeGuid);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        result.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo(new
        {
            Message = $"Invalid Guid format for registrationId : {fakeGuid}"
        });
    }

    [TestMethod]
    public void ThrowInvalidRegistrationId_ThrowsError__WhenNullRegistrationIdIsPassed()
    {
        // Arrange
        var materials = new List<MaterialDto>
        {
            new() { Code = "1", Name = "Wood" },
            new() { Code = "2", Name = "Plastic" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMaterialsByRegistrationIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(materials);

        // Act
        var result = _controller.ThrowInvalidRegistrationId(_loggerMock.Object, null);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}