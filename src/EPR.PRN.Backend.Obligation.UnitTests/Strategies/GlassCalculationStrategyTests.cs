using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Strategies;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies;

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
        var orgId = Guid.NewGuid();
        var calculationRequest = new SubmissionCalculationRequest
        {
            SubmissionPeriod = "2024-P4",
            PackagingMaterial = "GL",
            PackagingMaterialWeight = 200,
            OrganisationId = Guid.NewGuid()
        };

		var materials = new List<Material>()
        {
            new() {
                Id = 6,
                MaterialName = MaterialType.Glass.ToString(),
                MaterialCode = "GL"
            },
            new() {
			    Id = 7,
			    MaterialName = MaterialType.GlassRemelt.ToString(),
			    MaterialCode = "GR"
		    }
		};

		var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
        {
            {
                2025,
                new Dictionary<MaterialType, double>
                {
                    { MaterialType.Glass, 0.6 },
                    { MaterialType.GlassRemelt, 0.4 }
                }
            }
        };

        var request = new CalculationRequestDto
        {
            MaterialType = MaterialType.Glass,
			Materials = materials,
			SubmissionCalculationRequest = calculationRequest,
            OrganisationId = orgId,
            RecyclingTargets = recyclingTargets
        };

        _mockCalculationService.Setup(x => x.CalculateGlass(
            request.RecyclingTargets[2025][MaterialType.Glass], request.RecyclingTargets[2025][MaterialType.GlassRemelt],
            request.SubmissionCalculationRequest.PackagingMaterialWeight))
            .Returns((80, 120));  // remelt = 80, remainder = 120

        // Act
        var result = _strategy.Calculate(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

        var glassCalculation = result.Find(x => x.MaterialId == 6);
        var remeltCalculation = result.Find(x => x.MaterialId == 7);

        Assert.IsNotNull(glassCalculation);
        Assert.IsNotNull(remeltCalculation);

        Assert.AreEqual(120, glassCalculation.MaterialObligationValue);
        Assert.AreEqual(80, remeltCalculation.MaterialObligationValue);
        Assert.AreEqual(request.OrganisationId, glassCalculation.OrganisationId);
        Assert.AreEqual(request.OrganisationId, remeltCalculation.OrganisationId);
        Assert.AreEqual(DateTime.UtcNow.Year, glassCalculation.Year);
        Assert.AreEqual(DateTime.UtcNow.Year, remeltCalculation.Year);
        Assert.IsTrue((DateTime.UtcNow - glassCalculation.CalculatedOn).TotalSeconds < 1, "CalculatedOn should be very close to the current time");
        Assert.IsTrue((DateTime.UtcNow - remeltCalculation.CalculatedOn).TotalSeconds < 1, "CalculatedOn should be very close to the current time");
    }
}