using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

[TestClass]
public class RegistrationMaterialProfileMappingTests : MappingTestsBase<RegistrationMaterialProfile>
{
    [TestMethod]
    public void Registration_To_CreateRegistrationDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new Data.DataModels.Registrations.Registration
        {
            Id = 1,
            ExternalId = externalId
        };

        var expected = new CreateRegistrationDto
        {
            Id = externalId
        };

        // Act
        var result = mapper.Map<CreateRegistrationDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationMaterial_To_CreateRegistrationMaterialDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new RegistrationMaterial
        {
            Id = 1,
            ExternalId = externalId
        };

        var expected = new CreateRegistrationMaterialDto
        {
            Id = externalId
        };

        // Act
        var result = mapper.Map<CreateRegistrationMaterialDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationMaterial_To_ApplicantRegistrationMaterialDto_EnsureCorrectMapping()
    {
        // Arrange
        var registrationId = Guid.NewGuid();
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new RegistrationMaterial
        {
            Id = 1,
            ExternalId = externalId,
            Registration = new Data.DataModels.Registrations.Registration
            {
                Id = 1,
                ExternalId = registrationId
            },
            PPCPeriodId = 1,
            InstallationPeriodId = 1,
            WasteManagementPeriodId = 1,
            EnvironmentalPermitWasteManagementPeriodId = 1,
            EnvironmentalPermitWasteManagementNumber = "env",
            PPCPermitNumber = "ppc",
            WasteManagementLicenceNumber = "waste",
            InstallationPermitNumber = "install",
            PPCReprocessingCapacityTonne = 10,
            EnvironmentalPermitWasteManagementTonne = 20,
            InstallationReprocessingTonne = 30,
            WasteManagementReprocessingCapacityTonne = 40,
            MaximumReprocessingCapacityTonne = 100,
            PermitType = new LookupMaterialPermit
            {
                Name = "permitType",
                Id = 1
            },
            Material = new LookupMaterial
            {
                Id = 1,
                MaterialCode = "code",
                MaterialName = "plastic"
            },
            Status = new LookupRegistrationMaterialStatus
            {
                Name = "ready",
                Id = 1
            },
            MaterialExemptionReferences =
            [
                new()
                {
                    ReferenceNo = "exemption"
                }
            ],
            IsMaterialRegistered = true
        };

        var expected = new ApplicantRegistrationMaterialDto
        {
            Id = externalId,
            RegistrationId = registrationId,
            PPCReprocessingCapacityTonne = 10,
            EnvironmentalPermitWasteManagementTonne = 20,
            InstallationReprocessingTonne = 30,
            WasteManagementReprocessingCapacityTonne = 40,
            MaximumReprocessingCapacityTonne = 100,
            EnvironmentalPermitWasteManagementNumber = "env",
            PPCPermitNumber = "ppc",
            WasteManagementLicenceNumber = "waste",
            InstallationPermitNumber = "install",
            PPCPeriodId = 1,
            InstallationPeriodId = 1,
            WasteManagementPeriodId = 1,
            EnvironmentalPeriodId = 1,
            PermitType = new PermitTypeLookupDto
            {
                Id = 1,
                Name = "permitType"
            },
            IsMaterialRegistered = true,
            MaterialLookup = new MaterialLookupDto
            {
                Id = 1,
                Name = "plastic"
            },
            StatusLookup = new MaterialStatusLookupDto
            {
                Id = 1,
                Status = "ready"
            },
            ExemptionReferences =
            [
                new()
                {
                    ReferenceNumber = "exemption"
                }
            ]
        };

        // Act
        var result = mapper.Map<ApplicantRegistrationMaterialDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationMaterialContact_To_RegistrationMaterialContactDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new RegistrationMaterialContact
        {
            Id = 1,
            ExternalId = externalId
        };

        var expected = new RegistrationMaterialContactDto
        {
            Id = externalId
        };

        // Act
        var result = mapper.Map<RegistrationMaterialContactDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationReprocessingIOCommand_To_RegistrationReprocessingIORequestDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new RegistrationReprocessingIOCommand
        {
            ExternalId = externalId,
            TypeOfSuppliers = "Suppliers List"
        };

        var expected = new RegistrationReprocessingIORequestDto
        {
            ExternalId = externalId,
            TypeOfSuppliers = "Suppliers List"
        };

        // Act
        var result = mapper.Map<RegistrationReprocessingIORequestDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationReprocessingIORequestDto_To_RegistrationReprocessingIOCommand_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();

        var source = new RegistrationReprocessingIORequestDto
        {
            ExternalId = externalId,
            TypeOfSuppliers = "Suppliers List"
        };

        var expected = new RegistrationReprocessingIOCommand
        {
            ExternalId = externalId,
            TypeOfSuppliers = "Suppliers List"
        };

        // Act
        var result = mapper.Map<RegistrationReprocessingIOCommand>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationReprocessingIO_To_RegistrationReprocessingIOCommand_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();

        var source = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Suppliers List"
        };

        var expected = new RegistrationReprocessingIOCommand
        {
            TypeOfSuppliers = "Suppliers List"
        };

        // Act
        var result = mapper.Map<RegistrationReprocessingIOCommand>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void RegistrationReprocessingIOCommand_To_RegistrationReprocessingIO_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();

        var source = new RegistrationReprocessingIOCommand
        {
            TypeOfSuppliers = "Suppliers List"
        };

        var expected = new RegistrationReprocessingIO
        {
            TypeOfSuppliers = "Suppliers List"
        };

        // Act
        var result = mapper.Map<RegistrationReprocessingIO>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}