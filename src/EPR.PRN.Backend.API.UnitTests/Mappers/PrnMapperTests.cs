using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mappers;

[TestClass]
public class PrnMapperTests
{
    [TestMethod]
    public void ShouldMapSavePrnDetailsRequestToEprn()
    {
        var saveRequest = DataGenerator.CreateValidSavePrnDetailsRequest();
        var mapper = PrnMapper.CreateMapper();
        var result = mapper.Map<SavePrnDetailsRequest, Eprn>(saveRequest);
        result.Should().BeEquivalentTo(saveRequest);
    }
}
