using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using MediatR;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests;

[TestClass]
public class CreateMaterialExemptionReferenceHandlerTests
{
    [TestMethod]
    public async Task Handle_ShouldReturnTrue_WhenServiceReturnsTrue()
    {
        // Arrange
        var mockService = new Mock<IMaterialExemptionReferenceService>();
        var references = new List<MaterialExemptionReferenceRequest>
            {
                new MaterialExemptionReferenceRequest
                {
                    ExternalId = Guid.NewGuid(),
                    RegistrationMaterialId = 1,
                    ReferenceNumber = "REF123"
                }
        };
        var command = new CreateMaterialExemptionReferenceCommand
        {
            MaterialExemptionReferences = references
        };
        mockService
            .Setup(s => s.CreateMaterialExemptionReferenceAsync(references, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateMaterialExemptionReferenceHandler(mockService.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result);
        mockService.Verify(s => s.CreateMaterialExemptionReferenceAsync(references, It.IsAny<CancellationToken>()), Times.Once);
    }

    
[TestMethod]
    public async Task Handle_ShouldReturnFalse_WhenServiceReturnsFalse()
    {
        // Arrange  
        var mockService = new Mock<IMaterialExemptionReferenceService>();
        var references = new List<MaterialExemptionReferenceRequest>
        {
           new MaterialExemptionReferenceRequest
           {
               ExternalId = Guid.NewGuid(),
               RegistrationMaterialId = 0,
               ReferenceNumber = "REF456"
           }
        };
        var command = new CreateMaterialExemptionReferenceCommand
        {
            MaterialExemptionReferences = references
        };
        mockService
            .Setup(s => s.CreateMaterialExemptionReferenceAsync(references, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateMaterialExemptionReferenceHandler(mockService.Object);

        // Act  
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert  
        Assert.IsFalse(result);
        mockService.Verify(s => s.CreateMaterialExemptionReferenceAsync(references, It.IsAny<CancellationToken>()), Times.Once);
    }
}
