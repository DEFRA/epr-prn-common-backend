using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [TestClass]
    public class MaterialRepositoryTests
    {
        private MaterialRepository _materialRepository;
        private Mock<EprContext> _mockEprContext;
        private readonly List<Material> _materials =
            new List<Material>
            {
                new Material { MaterialCode = "PL", MaterialName = "Plastic" },
                new Material { MaterialCode = "WD", MaterialName = "Wood" },
                new Material { MaterialCode = "AL", MaterialName = "Aluminium" },
                new Material { MaterialCode = "ST", MaterialName = "Steel" },
                new Material { MaterialCode = "PC", MaterialName = "Paper" },
                new Material { MaterialCode = "GL", MaterialName = "Glass" }
            };

        [TestInitialize]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
            _mockEprContext = new Mock<EprContext>(dbContextOptions);
            _mockEprContext.Setup(context => context.Materials).ReturnsDbSet(_materials);
            _materialRepository = new MaterialRepository(_mockEprContext.Object);
        }

        [TestMethod]
        public async Task GetAllMaterials_ShouldReturnAllMaterials()
        {
            // Act
            var result = await _materialRepository.GetAllMaterials();

            // Assert
            result.Should().NotBeNull(); // Check that result is not null
            result.Should().HaveCount(6); // Check that 6 materials are returned
        }

        [TestMethod]
        public async Task GetAllMaterials_ShouldContainExpectedMaterialCodesAndNames()
        {
            // Act
            var result = await _materialRepository.GetAllMaterials();

            // Assert
            result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == "Plastic");
            result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == "Wood");
            result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == "Aluminium");
            result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == "Steel");
            result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == "Paper");
            result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == "Glass");
        }

        [TestMethod]
        public async Task GetAllMaterials_ShouldReturnMaterialsWithNonEmptyCodesAndNames()
        {
            // Act
            var result = await _materialRepository.GetAllMaterials();

            // Assert
            result.Should().OnlyContain(material => !string.IsNullOrWhiteSpace(material.MaterialCode));
            result.Should().OnlyContain(material => !string.IsNullOrWhiteSpace(material.MaterialName));
        }
    }
}
