using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests;

[TestClass]
public class MaterialExemptionReferenceControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<IValidationService> _validationServiceMock;
    private Mock<ILogger<MaterialExemptionReferenceController>> _loggerMock;
    private MaterialExemptionReferenceController _controller;


    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _validationServiceMock = new Mock<IValidationService>();
        _loggerMock = new Mock<ILogger<MaterialExemptionReferenceController>>();
        _controller = new MaterialExemptionReferenceController(_mediatorMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task SaveMaterialExemptionReference_ValidCommand_ReturnsTrue()
    {
        // Arrange  
        var command = new CreateMaterialExemptionReferenceCommand
        {
            MaterialExemptionReferences = new List<MaterialExemptionReferenceRequest>()
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateMaterialExemptionReferenceCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act  
        var result = await _controller.CreateMaterialExemptionReference(command);

        // Assert  
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<bool>();
        okResult.Value.As<bool>().Should().BeTrue(); 
    }

    [TestMethod]
    public async Task SaveMaterialExemptionReference_InvalidCommand_ReturnsFalse()
    {
        // Arrange  
        var command = new CreateMaterialExemptionReferenceCommand
        {
            MaterialExemptionReferences = new List<MaterialExemptionReferenceRequest>()
        };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateMaterialExemptionReferenceCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        // Act  
        var result = await _controller.CreateMaterialExemptionReference(command);
        // Assert  
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeOfType<bool>();
        okResult.Value.As<bool>().Should().BeFalse();
    }
}
