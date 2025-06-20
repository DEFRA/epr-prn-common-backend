using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetAllMaterialsHandlerTests
{
    private Mock<IMaterialRepository> _mockMaterialRepository;
    private Mock<IMaterialService> _mockMaterialService;
    private GetAllMaterialsQueryHandler _handler;
    private GetAllMaterialsQuery _query;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMaterialRepository = new Mock<IMaterialRepository>();
        _mockMaterialService = new Mock<IMaterialService>();
        _handler = new GetAllMaterialsQueryHandler(_mockMaterialService.Object);
        _query = new GetAllMaterialsQuery();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var materials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        var expectedMaterials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        _mockMaterialService
            .Setup(o => o.GetAllMaterialsAsync(CancellationToken.None))
            .ReturnsAsync(expectedMaterials);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMaterials);
    }
}