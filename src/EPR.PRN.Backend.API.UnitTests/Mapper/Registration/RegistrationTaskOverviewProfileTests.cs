using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Profiles.Regulator;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

[TestClass]
public class RegistrationTaskOverviewProfileTests : MappingTestsBase<RegistrationTaskOverviewProfile>
{
    [TestMethod]
    public void Registration_To_RegistrationTaskOverviewDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = CreateMapper();
        var source = new Data.DataModels.Registrations.Registration
        {
            Id = 1,
            ExternalId = externalId,
            ApplicationTypeId = 1
        };

        var expected = new RegistrationTaskOverviewDto
        {
            Id = externalId,
            Regulator = "EA"

        };

        // Act
        var result = mapper.Map<RegistrationTaskOverviewDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}