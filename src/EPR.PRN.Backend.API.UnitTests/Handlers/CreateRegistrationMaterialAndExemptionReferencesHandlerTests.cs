using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using EPR.PRN.Backend.Obligation.Models;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class CreateRegistrationMaterialAndExemptionReferencesHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _repositoryMock;
    private CreateRegistrationMaterialAndExemptionReferencesHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new CreateRegistrationMaterialAndExemptionReferencesHandler(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange  
        var command = new CreateRegistrationMaterialAndExemptionReferencesCommand
        {
            RegistrationMaterial = new RegistrationMaterialDto
            {
                ExternalId = Guid.NewGuid(),
                RegistrationId = 1,
                MaterialId = 10,
                MaterialName = "SampleMaterial",
                StatusId = 2,
                StatusUpdatedDate = DateTime.UtcNow,
                PermitTypeId = 3,
                PPCReprocessingCapacityTonne = 100.0m,
                WasteManagementReprocessingCapacityTonne = 200.0m,
                InstallationReprocessingTonne = 300.0m,
                EnvironmentalPermitWasteManagementTonne = 400.0m,
                MaximumReprocessingCapacityTonne = 500.0m,
                IsMaterialRegistered = true,
                CreatedDate = DateTime.UtcNow
            },
            MaterialExemptionReferences = new List<MaterialExemptionReferenceRequest>()
        };

        _repositoryMock
            .Setup(r => r.CreateRegistrationMaterialWithExemptionsAsync(It.IsAny<RegistrationMaterial>(), 
            It.IsAny<List<MaterialExemptionReference>>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act  
        await _handler.Handle(command, CancellationToken.None);

        // Assert  
        _repositoryMock.Verify();
        // Assert  
        _repositoryMock.Verify(r => r.CreateRegistrationMaterialWithExemptionsAsync(
            It.IsAny<RegistrationMaterial>(),
            It.IsAny<List<MaterialExemptionReference>>()),
            Times.Once);        
    }   
}
