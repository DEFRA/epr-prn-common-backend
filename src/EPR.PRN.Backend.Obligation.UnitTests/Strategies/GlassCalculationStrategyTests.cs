using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Strategies;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies
{
    [TestClass]
    public class GlassCalculationStrategyTests
    {
        private Mock<IMaterialCalculationService> _mockCalculationService;
        private GlassCalculationStrategy _strategy;

        [TestInitialize]
        public void SetUp()
        {
            _mockCalculationService = new Mock<IMaterialCalculationService>();
            _strategy = new GlassCalculationStrategy(_mockCalculationService.Object);
        }

        [TestMethod]
        public void CanHandle_ShouldReturnTrue_ForGlassMaterial()
        {
            // Act
            var result = _strategy.CanHandle(MaterialType.Glass);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow(MaterialType.Plastic)]
        [DataRow(MaterialType.Aluminium)]
        [DataRow(MaterialType.Paper)]
        [DataRow(MaterialType.GlassRemelt)]
        public void CanHandle_ShouldReturnFalse_ForNonGlassMaterials(MaterialType materialType)
        {
            // Act
            var result = _strategy.CanHandle(materialType);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Calculate_ShouldReturnCorrectObligationCalculations()
        {
            // Arrange
            var pomObligation = new PomObligtionDto
            {
                PackagingMaterial = "Glass",
                PackagingMaterialWeight = 200,
                OrganisationId = 1
            };

            var materialType = MaterialType.Glass;

            var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
            {
                {
                    DateTime.UtcNow.Year,
                    new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Glass, 0.6 },
                        { MaterialType.GlassRemelt, 0.4 }
                    }
                }
            };

            _mockCalculationService.Setup(x => x.CalculateGlass(0.6, 0.4, 200))
                .Returns((80, 120));  // remelt = 80, remainder = 120

            // Act
            var result = _strategy.Calculate(pomObligation, materialType, recyclingTargets);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var glassCalculation = result.Find(x => x.MaterialName == MaterialType.Glass.ToString());
            var remeltCalculation = result.Find(x => x.MaterialName == MaterialType.GlassRemelt.ToString());

            Assert.IsNotNull(glassCalculation);
            Assert.IsNotNull(remeltCalculation);

            Assert.AreEqual(120, glassCalculation.MaterialObligationValue);
            Assert.AreEqual(80, remeltCalculation.MaterialObligationValue);
            Assert.AreEqual(pomObligation.OrganisationId, glassCalculation.OrganisationId);
            Assert.AreEqual(pomObligation.OrganisationId, remeltCalculation.OrganisationId);
            Assert.AreEqual(DateTime.UtcNow.Year, glassCalculation.Year);
            Assert.AreEqual(DateTime.UtcNow.Year, remeltCalculation.Year);
            Assert.IsTrue((DateTime.UtcNow - glassCalculation.CalculatedOn).TotalSeconds < 1, "CalculatedOn should be very close to the current time");
            Assert.IsTrue((DateTime.UtcNow - remeltCalculation.CalculatedOn).TotalSeconds < 1, "CalculatedOn should be very close to the current time");
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Calculate_ShouldThrowKeyNotFoundException_WhenRecyclingTargetYearNotFound()
        {
            // Arrange
            var pomObligation = new PomObligtionDto
            {
                PackagingMaterial = "Glass",
                PackagingMaterialWeight = 200,
                OrganisationId = 1
            };

            var materialType = MaterialType.Glass;

            var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
            {
                {
                    DateTime.UtcNow.Year - 1, // Use a past year to trigger the exception
                    new Dictionary<MaterialType, double>
                    {
                        { MaterialType.Glass, 0.6 },
                        { MaterialType.GlassRemelt, 0.4 }
                    }
                }
            };

            // Act
            _strategy.Calculate(pomObligation, materialType, recyclingTargets);

            // Assert is handled by ExpectedException
        }
    }
}