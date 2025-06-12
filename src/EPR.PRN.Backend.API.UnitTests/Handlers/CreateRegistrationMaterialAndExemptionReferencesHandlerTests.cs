using AutoMapper;
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
    private Mock<IMapper> _mapperMock;
    private CreateRegistrationMaterialAndExemptionReferencesHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRegistrationMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateRegistrationMaterialAndExemptionReferencesHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange  
        var command = new CreateRegistrationMaterialAndExemptionReferencesCommand
        {
            RegistrationMaterial = new RegistrationMaterialDto
            {
                RegistrationId = Guid.NewGuid(),
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
            MaterialExemptionReferences = new List<MaterialExemptionReferenceDto>()
        };

        var mappedMaterial = new RegistrationMaterial
        {
            MaterialId = command.RegistrationMaterial.MaterialId,
            StatusId = command.RegistrationMaterial.StatusId,
            PermitTypeId = command.RegistrationMaterial.PermitTypeId,
            PPCReprocessingCapacityTonne = command.RegistrationMaterial.PPCReprocessingCapacityTonne,
            WasteManagementReprocessingCapacityTonne = command.RegistrationMaterial.WasteManagementReprocessingCapacityTonne,
            InstallationReprocessingTonne = command.RegistrationMaterial.InstallationReprocessingTonne,
            EnvironmentalPermitWasteManagementTonne = command.RegistrationMaterial.EnvironmentalPermitWasteManagementTonne,
            MaximumReprocessingCapacityTonne = command.RegistrationMaterial.MaximumReprocessingCapacityTonne,
            IsMaterialRegistered = command.RegistrationMaterial.IsMaterialRegistered,
            CreatedDate = command.RegistrationMaterial.CreatedDate
        };

        var mappedExemptions = new List<MaterialExemptionReference>();

        _mapperMock
            .Setup(m => m.Map<RegistrationMaterial>(command.RegistrationMaterial))
            .Returns(mappedMaterial);

        _mapperMock
            .Setup(m => m.Map<List<MaterialExemptionReference>>(command.MaterialExemptionReferences))
            .Returns(mappedExemptions);

        _repositoryMock
            .Setup(r => r.CreateRegistrationMaterialWithExemptionsAsync(It.IsAny<RegistrationMaterial>(),
            It.IsAny<List<MaterialExemptionReference>>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act  
        await _handler.Handle(command, CancellationToken.None);

        // Assert  
        _repositoryMock.Verify();
        _repositoryMock.Verify(r => r.CreateRegistrationMaterialWithExemptionsAsync(
            mappedMaterial,
            mappedExemptions),
            Times.Once);
    }
    
    [TestMethod]
    public async Task Handle_ShouldPassCorrectRegistrationMaterialAndExemptionReferencesToRepository()
    {
        // Arrange
        var expectedMaterial = new RegistrationMaterialDto
        {
            RegistrationId = Guid.NewGuid(),
            MaterialId = 99,
            MaterialName = "TestMaterial",
            StatusId = 5,
            StatusUpdatedDate = DateTime.UtcNow.AddDays(-1),
            PermitTypeId = 7,
            PPCReprocessingCapacityTonne = 123.45m,
            WasteManagementReprocessingCapacityTonne = 234.56m,
            InstallationReprocessingTonne = 345.67m,
            EnvironmentalPermitWasteManagementTonne = 456.78m,
            MaximumReprocessingCapacityTonne = 567.89m,
            IsMaterialRegistered = false,
            CreatedDate = DateTime.UtcNow.AddDays(-2)
        };

        var expectedExemptions = new List<MaterialExemptionReferenceDto>
        {
            new() { ReferenceNumber = "EX-001" },
            new() { ReferenceNumber = "EX-002" }
        };

        var command = new CreateRegistrationMaterialAndExemptionReferencesCommand
        {
            RegistrationMaterial = expectedMaterial,
            MaterialExemptionReferences = expectedExemptions
        };

        var mappedMaterial = new RegistrationMaterial
        {
            MaterialId = expectedMaterial.MaterialId,
            StatusId = expectedMaterial.StatusId,
            PermitTypeId = expectedMaterial.PermitTypeId,
            PPCReprocessingCapacityTonne = expectedMaterial.PPCReprocessingCapacityTonne,
            WasteManagementReprocessingCapacityTonne = expectedMaterial.WasteManagementReprocessingCapacityTonne,
            InstallationReprocessingTonne = expectedMaterial.InstallationReprocessingTonne,
            EnvironmentalPermitWasteManagementTonne = expectedMaterial.EnvironmentalPermitWasteManagementTonne,
            MaximumReprocessingCapacityTonne = expectedMaterial.MaximumReprocessingCapacityTonne,
            IsMaterialRegistered = expectedMaterial.IsMaterialRegistered,
            CreatedDate = expectedMaterial.CreatedDate
        };

        var mappedExemptions = expectedExemptions
            .Select(dto => new MaterialExemptionReference { ReferenceNo = dto.ReferenceNumber })
            .ToList();

        RegistrationMaterial actualMaterial = null;
        List<MaterialExemptionReference> actualExemptions = null;

        _mapperMock
            .Setup(m => m.Map<RegistrationMaterial>(expectedMaterial))
            .Returns(mappedMaterial);

        _mapperMock
            .Setup(m => m.Map<List<MaterialExemptionReference>>(expectedExemptions))
            .Returns(mappedExemptions);

        _repositoryMock
            .Setup(r => r.CreateRegistrationMaterialWithExemptionsAsync(
                It.IsAny<RegistrationMaterial>(),
                It.IsAny<List<MaterialExemptionReference>>()))
            .Callback<RegistrationMaterial, List<MaterialExemptionReference>>((mat, exs) =>
            {
                actualMaterial = mat;
                actualExemptions = exs;
            })
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(actualMaterial);
        Assert.AreEqual(command.RegistrationMaterial.MaterialId, actualMaterial.MaterialId);
        Assert.AreEqual(command.RegistrationMaterial.StatusId, actualMaterial.StatusId);
        Assert.AreEqual(command.RegistrationMaterial.PermitTypeId, actualMaterial.PermitTypeId);
        Assert.AreEqual(command.RegistrationMaterial.PPCReprocessingCapacityTonne, actualMaterial.PPCReprocessingCapacityTonne);
        Assert.AreEqual(command.RegistrationMaterial.WasteManagementReprocessingCapacityTonne, actualMaterial.WasteManagementReprocessingCapacityTonne);
        Assert.AreEqual(command.RegistrationMaterial.InstallationReprocessingTonne, actualMaterial.InstallationReprocessingTonne);
        Assert.AreEqual(command.RegistrationMaterial.EnvironmentalPermitWasteManagementTonne, actualMaterial.EnvironmentalPermitWasteManagementTonne);
        Assert.AreEqual(command.RegistrationMaterial.MaximumReprocessingCapacityTonne, actualMaterial.MaximumReprocessingCapacityTonne);
        Assert.AreEqual(command.RegistrationMaterial.IsMaterialRegistered, actualMaterial.IsMaterialRegistered);
        Assert.AreEqual(command.RegistrationMaterial.CreatedDate, actualMaterial.CreatedDate);

        Assert.IsNotNull(actualExemptions);
        Assert.AreEqual(expectedExemptions.Count, actualExemptions.Count);

        for (int i = 0; i < expectedExemptions.Count; i++)
        {
            Assert.AreEqual(expectedExemptions[i].ReferenceNumber, actualExemptions[i].ReferenceNo);
        }
    }
}
