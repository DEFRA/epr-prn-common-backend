using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Services;

[TestClass]
public class MaterialServiceTests
{
    private MaterialService _systemUnderTest;
    private Mock<IMaterialRepository> _mockRepository;
    private Mock<ILogger<MaterialService>> _mockLogger;
    private Mock<IConfiguration> _configurationMock;

    [TestInitialize]
    public void Init()
    {
        _mockRepository = new Mock<IMaterialRepository>();
        _mockLogger = new Mock<ILogger<MaterialService>>();
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(c => c["LogPrefix"]).Returns("[EPR.PRN.Backend]");

        _systemUnderTest = new MaterialService(_mockLogger.Object, _mockRepository.Object);
    }

    [TestMethod]
    public async Task GetAllMaterials_EnsureDataReturned()
    {
        // Arrange
        var materials = new List<Material>
        {
            new() { MaterialName = "Wood", MaterialCode = "W1" },
            new() { MaterialName = "Plastic", MaterialCode = "P1" }
        };

        var expectedMaterials = new List<MaterialDto>
        {
            new() { Name = "Wood", Code = "W1" },
            new() { Name = "Plastic", Code = "P1" }
        };

        // Expectations
        _mockRepository.Setup(r => r.GetAllMaterials()).ReturnsAsync(materials);

        // Act
        var result = await _systemUnderTest.GetAllMaterialsAsync(CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedMaterials);
    }
}