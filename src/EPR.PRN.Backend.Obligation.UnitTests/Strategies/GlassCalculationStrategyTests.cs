using AutoFixture.MSTest;
using EPR.PRN.Backend.Obligation.DTO;
using EPR.PRN.Backend.Obligation.Enums;
using EPR.PRN.Backend.Obligation.Helpers;
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
    [AutoData]
    public void Calculate_ShouldReturnCorrectObligationCalculations(CalculationRequestDto calculationRequest)
    {
        // Arrange
        calculationRequest.RecyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
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

        calculationRequest.SubmissionCalculationRequest.SubmissionPeriod = "2024-P4";
        var targetYear = DateHelper.ExtractYear(calculationRequest.SubmissionCalculationRequest.SubmissionPeriod);

        _mockCalculationService.Setup(x => x.CalculateGlass(
            calculationRequest.RecyclingTargets[DateTime.Now.Year][MaterialType.Glass], calculationRequest.RecyclingTargets[targetYear][MaterialType.GlassRemelt],
            calculationRequest.SubmissionCalculationRequest.PackagingMaterialWeight))
            .Returns((80, 120));  // remelt = 80, remainder = 120

        // Act
        var result = _strategy.Calculate(calculationRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);

        var glassCalculation = result.Find(x => x.MaterialName == MaterialType.Glass.ToString());
        var remeltCalculation = result.Find(x => x.MaterialName == MaterialType.GlassRemelt.ToString());

        Assert.IsNotNull(glassCalculation);
        Assert.IsNotNull(remeltCalculation);

        Assert.AreEqual(120, glassCalculation.MaterialObligationValue);
        Assert.AreEqual(80, remeltCalculation.MaterialObligationValue);
        Assert.AreEqual(calculationRequest.OrganisationId, glassCalculation.OrganisationId);
        Assert.AreEqual(calculationRequest.OrganisationId, remeltCalculation.OrganisationId);
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
        var calculationRequest = new SubmissionCalculationRequest
        {
            SubmissionPeriod = "2024-P4",
            PackagingMaterial = "Glass",
            PackagingMaterialWeight = 200,
            SubmissionId = Guid.NewGuid()
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

        var request = new CalculationRequestDto { MaterialType = materialType, RecyclingTargets = recyclingTargets, SubmissionCalculationRequest = calculationRequest };

        // Act
        _strategy.Calculate(request);

        // Assert is handled by ExpectedException
    }
}