using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Services;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services
{
    [TestClass]
    public class MaterialServiceTests
    {
        private Mock<IMaterialRepository> _materialRepository;
        private MaterialService _service;

        [TestInitialize]
        public void Setup()
        {
            _materialRepository = new Mock<IMaterialRepository>();
            _service = new MaterialService(_materialRepository.Object);
        }

        [TestMethod]
        public void GetMaterialByCode_ValidCode_ShouldReturnCorrectMaterialType()
        {
            // Arrange
            var code = "PL"; // Plastic
            var materials = new List<Materials>
            {
                new Materials { MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString() }
            };
            _materialRepository.Setup(repo => repo.GetAllMaterials()).Returns(materials);

            // Act
            var result = _service.GetMaterialByCode(code);

            // Assert
            result.Should().Be(MaterialType.Plastic);
        }

        [TestMethod]
        public void GetMaterialByCode_InvalidCode_ShouldReturnNull()
        {
            // Arrange
            var code = "INVALID";

            // Act
            var result = _service.GetMaterialByCode(code);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetMaterialByCode_EmptyCode_ShouldReturnNull()
        {
            // Arrange
            var code = "";

            // Act
            var result = _service.GetMaterialByCode(code);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetMaterialByCode_NullCode_ShouldReturnNull()
        {
            // Arrange
            string code = null;

            // Act
            var result = _service.GetMaterialByCode(code);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetMaterialByCode_ValidCodeButInvalidMaterialType_ShouldReturnNull()
        {
            // Arrange
            var code = "XX"; // Code that doesn't map to a valid Enum value
            var materials = new List<Materials>
            {
                new Materials { MaterialCode = "XX", MaterialName = "UnknownMaterial" }
            };
            _materialRepository.Setup(repo => repo.GetAllMaterials()).Returns(materials);

            // Act
            var result = _service.GetMaterialByCode(code);

            // Assert
            result.Should().BeNull();
        }
    }
}
