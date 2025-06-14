using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Profiles.Regulator;
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
        var source = new Data.DataModels.Registrations.RegistrationMaterial
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
}