using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Profiles;
using EPR.PRN.Backend.Data.DataModels;
using FluentAssertions;

namespace EPR.PRN.Backend.API.UnitTests.Mappers;

[TestClass]
public class MapperTests
{
    [TestMethod]
    public void ShouldMapSavePrnDetailsRequestToEprn()
    {
        var saveRequest = DataGenerator.CreateValidSavePrnDetailsRequestV2();
        var mapper = PrnMapper.CreateMapper();
        var result = mapper.Map<SavePrnDetailsRequestV2, Eprn>(saveRequest);
        result.Should().BeEquivalentTo(saveRequest);
    }
}
