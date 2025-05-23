using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialWasteLicenceQueryHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private IMapper _mapper;
    private GetMaterialWasteLicencesQueryHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegistrationMaterialProfile>();
        });
        _mapper = config.CreateMapper();

        _handler = new GetMaterialWasteLicencesQueryHandler(_rmRepositoryMock.Object, _mapper);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialWithExemptionsExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.WasteExemption },
            MaterialExemptionReferences = new List<MaterialExemptionReference>
            {
                new MaterialExemptionReference
                {
                    ReferenceNo = "1"
                },
                new MaterialExemptionReference
                {
                    ReferenceNo = "2"
                }
            },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year"},
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.WasteExemption);
            result.LicenceNumbers.First().Should().Be("1");
            result.LicenceNumbers.Skip(1).First().Should().Be("2");
            result.LicenceNumbers.Length.Should().Be(2);
            result.CapacityTonne.Should().BeNull();
            result.CapacityPeriod.Should().BeNull();
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }

    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialWithExemptionsExistsNoReference()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.WasteExemption },
            MaterialExemptionReferences = null,
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.WasteExemption);
            result.LicenceNumbers.Length.Should().Be(0);
            result.CapacityTonne.Should().BeNull();
            result.CapacityPeriod.Should().BeNull();
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }

    }


    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialWasteManagementLicenceExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.WasteManagementLicence },
            WasteManagementLicenceNumber = "1",
            WasteManagementReprocessingCapacityTonne = 456,
            WasteManagementPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.WasteManagementLicence);
            result.LicenceNumbers.First().Should().Be("1");
            result.LicenceNumbers.Length.Should().Be(1);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialWasteManagementLicenceExistsNoReference()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.WasteManagementLicence },
            WasteManagementLicenceNumber = null,
            WasteManagementReprocessingCapacityTonne = 456,
            WasteManagementPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.WasteManagementLicence);
            result.LicenceNumbers.Length.Should().Be(0);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialEnvironmentalPermitOrWasteManagementLicenceExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.EnvironmentalPermitOrWasteManagementLicence },
            EnvironmentalPermitWasteManagementNumber = "1",
            EnvironmentalPermitWasteManagementTonne = 456,
            EnvironmentalPermitWasteManagementPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,

            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.EnvironmentalPermitOrWasteManagementLicence);
            result.LicenceNumbers.First().Should().Be("1");
            result.LicenceNumbers.Length.Should().Be(1);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialEnvironmentalPermitOrWasteManagementLicenceExistsNoReference()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.EnvironmentalPermitOrWasteManagementLicence },
            EnvironmentalPermitWasteManagementNumber = null,
            EnvironmentalPermitWasteManagementTonne = 456,
            EnvironmentalPermitWasteManagementPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,

            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.EnvironmentalPermitOrWasteManagementLicence);
            result.LicenceNumbers.Length.Should().Be(0);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialInstallationPermitExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.InstallationPermit },
            InstallationPermitNumber = "1",
            InstallationReprocessingTonne = 456,
            InstallationPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.InstallationPermit);
            result.LicenceNumbers.First().Should().Be("1");
            result.LicenceNumbers.Length.Should().Be(1);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialInstallationPermitExistsNoReference()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.InstallationPermit },
            InstallationPermitNumber = null,
            InstallationReprocessingTonne = 456,
            InstallationPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.InstallationPermit);
            result.LicenceNumbers.Length.Should().Be(0);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialPollutionPreventionAndControlPermitExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.PollutionPreventionAndControlPermit },
            PPCPermitNumber = "1",
            PPCReprocessingCapacityTonne = 456,
            PPCPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.PollutionPreventionAndControlPermit);
            result.LicenceNumbers.First().Should().Be("1");
            result.LicenceNumbers.Length.Should().Be(1);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialPollutionPreventionAndControlPermitExistsNoReference()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = new LookupMaterialPermit { Name = PermitTypes.PollutionPreventionAndControlPermit },
            PPCPermitNumber = null,
            PPCReprocessingCapacityTonne = 456,
            PPCPeriod = new LookupPeriod { Name = "Per Year" },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.MaterialName.Should().Be("Plastic");
            result.PermitType.Should().Be(PermitTypes.PollutionPreventionAndControlPermit);
            result.LicenceNumbers.Length.Should().Be(0);
            result.CapacityTonne.Should().Be(456);
            result.CapacityPeriod.Should().Be("Per Year");
            result.MaximumReprocessingCapacityTonne.Should().Be(123);
            result.MaximumReprocessingPeriod.Should().Be("Per Year");
        }
    }

    [TestMethod]
    public async Task Handle_ShouldThrowRegulatorInvalidOperationException_WhenMaterialNullPermitExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            ExternalId = materialId,
            Registration = new Registration { ExternalId = Guid.Parse("4bac12f7-f7a9-4df4-b7b5-9c4221860c4d") },
            MaterialId = 2,
            Material = new LookupMaterial { MaterialName = "Plastic" },
            StatusId = 1,
            Status = new LookupRegistrationMaterialStatus { Id = 1, Name = "Granted" },
            PermitType = null,
            MaterialExemptionReferences = new List<MaterialExemptionReference>
            {
                new MaterialExemptionReference
                {
                    ReferenceNo = "1"
                },
                new MaterialExemptionReference
                {
                    ReferenceNo = "2"
                }
            },
            MaximumReprocessingCapacityTonne = 123,
            MaximumReprocessingPeriod = new LookupPeriod { Name = "Per Year" },
        };

        _rmRepositoryMock
            .Setup(r => r.GetRegistrationMaterial_WasteLicencesById(materialId))
            .ReturnsAsync(materialEntity);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AutoMapperMappingException>()
            .WithInnerException(typeof(RegulatorInvalidOperationException))
            .WithMessage("Permit Type Not Valid");
    }
}