using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto.Regulator;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers;

[TestClass]
public class RegistrationMaterialControllerTests
{
    private Mock<IMediator> _mediatorMock;
    private Mock<ILogger<RegistrationMaterialController>> _loggerMock;
    private RegistrationMaterialController _controller;

    [TestInitialize]
    public void TestInitialize()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<RegistrationMaterialController>>();
        _controller = new RegistrationMaterialController(_mediatorMock.Object, _loggerMock.Object);
    }
    
    [TestMethod]
    public async Task CreateExemptionReferences_ValidCommand_ReturnsNoContent()
    {
        // Arrange  
        var command = new CreateExemptionReferencesCommand
        {           
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>()
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateExemptionReferencesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act  
        var result = await _controller.CreateExemptionReferences(command);

        // Assert  
        result.Should().BeOfType<OkResult>();
       

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true), 
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce
        );
    }

    [TestMethod]
    public async Task CreateRegistrationMaterialExpectedCreatedResult()
    {
        // Arrange
        var expectedResult = new CreatedResult(string.Empty, 10);
        var validator = new InlineValidator<RegistrationMaterialsOutcomeCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        var registrationId = 1;
        var command = new CreateRegistrationMaterialCommand
        {
            RegistrationId = 1,
            Material = "Steel"
        };

        // Expectations
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(10);

        // Act
        var result = await _controller.CreateRegistrationMaterial(command);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
