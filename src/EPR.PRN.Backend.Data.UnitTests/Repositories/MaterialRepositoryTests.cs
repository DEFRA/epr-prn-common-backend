using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [TestClass]
    public class MaterialRepositoryTests
    {
        private MaterialRepository _materialRepository;

        [TestInitialize]
        public void Setup()
        {
            _materialRepository = new MaterialRepository();
        }

        [TestMethod]
        public void GetAllMaterials_ShouldReturnAllMaterials()
        {
            // Act
            var result = _materialRepository.GetAllMaterials();

            // Assert
            result.Should().NotBeNull(); // Check that result is not null
            result.Should().HaveCount(6); // Check that 6 materials are returned
        }

        [TestMethod]
        public void GetAllMaterials_ShouldContainExpectedMaterialCodesAndNames()
        {
            // Act
            var result = _materialRepository.GetAllMaterials().ToList();

            // Assert
            result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == "Plastic");
            result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == "Wood");
            result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == "Aluminium");
            result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == "Steel");
            result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == "Paper");
            result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == "Glass");
        }

        [TestMethod]
        public void GetAllMaterials_ShouldReturnMaterialsWithNonEmptyCodesAndNames()
        {
            // Act
            var result = _materialRepository.GetAllMaterials();

            // Assert
            result.Should().OnlyContain(material => !string.IsNullOrWhiteSpace(material.MaterialCode));
            result.Should().OnlyContain(material => !string.IsNullOrWhiteSpace(material.MaterialName));
        }
    }
}
