using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
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
    public async Task CreateRegistrationMaterialAndExemptionReferences_ValidCommand_ReturnsNoContent()
    {
        // Arrange  
        var command = new CreateRegistrationMaterialAndExemptionReferencesCommand
        {
            RegistrationMaterial = new RegistrationMaterialDto
            {
                MaterialName = "Test Material"
            },
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>()
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateRegistrationMaterialAndExemptionReferencesCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Unit.Value));

        // Act  
        var result = await _controller.CreateRegistrationMaterialAndExemptionReferences(command);

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
        var externalId = Guid.NewGuid();
        var expectedResult = new CreatedResult(string.Empty, new CreateRegistrationMaterialDto
        {
            Id = externalId
        });
        var validator = new InlineValidator<RegistrationMaterialsOutcomeCommand>();
        validator.RuleFor(x => x.Status).Must(_ => false).WithMessage("Validation failed");

        var command = new CreateRegistrationMaterialCommand
        {
            RegistrationId = externalId,
            Material = "Steel"
        };

        // Expectations
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateRegistrationMaterialDto
            {
                Id = externalId
            });

        // Act
        var result = await _controller.CreateRegistrationMaterial(command);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
