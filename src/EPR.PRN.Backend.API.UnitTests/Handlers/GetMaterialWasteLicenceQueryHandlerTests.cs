using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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
        int materialId = 1;
        var query = new GetMaterialWasteLicencesQuery { Id = materialId };

        var materialEntity = new RegistrationMaterial
        {
            Id = materialId,
            RegistrationId = 10,
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