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
}