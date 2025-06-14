using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class CreateExemptionReferencesHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;    
    private CreateExemptionReferencesHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();        
        _handler = new CreateExemptionReferencesHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange  
        var registrationMaterialId = Guid.NewGuid();
        var command = new CreateExemptionReferencesCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>()
        };
      
        var mappedExemptions = command.MaterialExemptionReferences
            .Select(dto => new MaterialExemptionReference { ReferenceNo = dto.ReferenceNumber })
            .ToList();

        _repositoryMock
            .Setup(r => r.CreateExemptionReferencesAsync(
                It.IsAny<Guid>(),
                It.Is<List<MaterialExemptionReference>>(l => l.Count == mappedExemptions.Count)))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act  
        await _handler.Handle(command, CancellationToken.None);

        // Assert  
        _repositoryMock.Verify();
        _repositoryMock.Verify(r => r.CreateExemptionReferencesAsync(
            It.IsAny<Guid>(),
            It.Is<List<MaterialExemptionReference>>(l => l.Count == mappedExemptions.Count)),
            Times.Once);
    }
    
    [TestMethod]
    public async Task Handle_ShouldPassCorrectExemptionReferencesToRepository()
    {
        // Arrange        
        var registrationMaterialId = Guid.NewGuid();
        var expectedExemptions = new List<MaterialExemptionReferenceDto>
        {
            new() { ReferenceNumber = "EX-001" },
            new() { ReferenceNumber = "EX-002" }
        };

        var command = new CreateExemptionReferencesCommand
        {
            RegistrationMaterialId = registrationMaterialId,
            MaterialExemptionReferences = expectedExemptions
        };

        List<MaterialExemptionReference> actualExemptions = null;
        Guid actualRegistrationMaterialId = Guid.Empty;

        _repositoryMock
            .Setup(r => r.CreateExemptionReferencesAsync(It.IsAny<Guid>(), It.IsAny<List<MaterialExemptionReference>>()))
            .Callback<Guid, List<MaterialExemptionReference>>((id, exs) =>
            {
                actualRegistrationMaterialId = id;
                actualExemptions = exs;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(actualExemptions);        
        Assert.AreEqual(expectedExemptions.Count, actualExemptions.Count);

        for (int i = 0; i < expectedExemptions.Count; i++)
        {
            Assert.AreEqual(expectedExemptions[i].ReferenceNumber, actualExemptions[i].ReferenceNo);
        }
    }
}
