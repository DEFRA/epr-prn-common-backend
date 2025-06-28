using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Services.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialsByRegistrationIdQueryHandlerTests
{
    private Mock<IMaterialRepository> _mockMaterialRepository;
    private Mock<IMaterialService> _mockMaterialService;
    private GetMaterialsByRegistrationIdQueryHandler _handler;
    private GetMaterialsByRegistrationIdQuery _query;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMaterialRepository = new Mock<IMaterialRepository>();
        _mockMaterialService = new Mock<IMaterialService>();
        _handler = new GetMaterialsByRegistrationIdQueryHandler(_mockMaterialService.Object);
        _query = new GetMaterialsByRegistrationIdQuery();
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange

        var registrationId = Guid.NewGuid();

        _query = new GetMaterialsByRegistrationIdQuery() { RegistrationId = registrationId };

        var expectedMaterials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        _mockMaterialService
            .Setup(o => o.GetMaterialsByRegistrationIdQuery(registrationId))
            .ReturnsAsync(expectedMaterials);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMaterials);
    }
}