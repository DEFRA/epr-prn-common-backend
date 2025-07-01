using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

[TestClass]
public class RegistrationTaskOverviewProfileTests : MappingProfileTestBase
{
    [TestMethod]
    public void Registration_To_RegistrationTaskOverviewDto_EnsureCorrectMapping()
    {
        // Arrange
        var externalId = Guid.NewGuid();
        var mapper = Mapper.CreateMapper();
        var source = new Data.DataModels.Registrations.Registration
        {
            Id = 1,
            ExternalId = externalId,
            ApplicationTypeId = 1,
            Materials = new List<RegistrationMaterial>
            {
                new ()
                {
                    Id = 1,
                    Material = new LookupMaterial
                    {
                        Id = 1, MaterialCode = "STL", MaterialName = "Steel"
                    },
                    IsMaterialRegistered = true,
                    StatusUpdatedDate = new DateTime(2020, 01, 01, 12, 00, 00)
                }
            }
        };

        var expected = new RegistrationTaskOverviewDto
        {
            Id = externalId,
            Regulator = "EA",
            Materials = new List<RegistrationMaterialDto>
            {
                new ()
                {
                    MaterialName = "Steel",
                    IsMaterialRegistered = true,
                    StatusUpdatedDate = new DateTime(2020, 01, 01, 12, 00, 00),
                    Comments = ""
                }
            }

        };

        // Act
        var result = mapper.Map<RegistrationTaskOverviewDto>(source);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}