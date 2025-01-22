using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Strategies;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies;

[TestClass]
public class GeneralCalculationStrategyTests
{
    private Mock<IMaterialCalculationService> _mockCalculationService;
    private GeneralCalculationStrategy _strategy;

    [TestInitialize]
    public void SetUp()
    {
        _mockCalculationService = new Mock<IMaterialCalculationService>();
        _strategy = new GeneralCalculationStrategy(_mockCalculationService.Object);
    }

    [TestMethod]
    [DataRow(MaterialType.Plastic)]
    [DataRow(MaterialType.Aluminium)]
    [DataRow(MaterialType.Paper)]
    public void CanHandle_ShouldReturnTrue_ForGeneralMaterials(MaterialType materialType)
    {
        // Act
        var result = _strategy.CanHandle(materialType);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    [DataRow(MaterialType.Glass)]
    [DataRow(MaterialType.GlassRemelt)]
    public void CanHandle_ShouldReturnFalse_ForNonGeneralMaterials(MaterialType materialType)
    {
        // Act
        var result = _strategy.CanHandle(materialType);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Calculate_ShouldReturnCorrectObligationCalculation()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var calculationRequest = new SubmissionCalculationRequest
        {
            PackagingMaterial = "Plastic",
            PackagingMaterialWeight = 100,
            SubmissionId = Guid.NewGuid(),
            SubmissionPeriod = "2024-P1"
        };

        var materialType = MaterialType.Plastic;

        var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
        {
            {
                2025,
                new Dictionary<MaterialType, double>
                {
                    { materialType, 0.7 }
                }
            }
        };

        var request = new CalculationRequestDto { SubmissionCalculationRequest = calculationRequest, RecyclingTargets = recyclingTargets, MaterialType = materialType, OrganisationId = organisationId };

        _mockCalculationService.Setup(x => x.Calculate(0.7, 100)).Returns(70);

        // Act
        var result = _strategy.Calculate(request);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("Plastic", result[0].MaterialName);
        Assert.AreEqual(70, result[0].MaterialObligationValue);
        Assert.AreEqual(organisationId, result[0].OrganisationId);
        Assert.AreEqual(DateTime.UtcNow.Year, result[0].Year);
        Assert.AreEqual(calculationRequest.PackagingMaterialWeight, result[0].Tonnage);
        Assert.IsTrue((DateTime.UtcNow - result[0].CalculatedOn).TotalSeconds < 1, "CalculatedOn should be very close to the current time");
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void Calculate_ShouldThrowKeyNotFoundException_WhenRecyclingTargetYearNotFound()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var calculationRequest = new SubmissionCalculationRequest
        {
            PackagingMaterial = "Plastic",
            PackagingMaterialWeight = 100,
            SubmissionId = Guid.NewGuid(),
            SubmissionPeriod = "2025-P1"
        };

        var materialType = MaterialType.Plastic;

        var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
        {
            {
                DateTime.UtcNow.Year - 1, // Use a past year to trigger the exception
                new Dictionary<MaterialType, double>
                {
                    { materialType, 0.7 }
                }
            }
        };
        var request = new CalculationRequestDto { SubmissionCalculationRequest = calculationRequest, RecyclingTargets = recyclingTargets, MaterialType = materialType, OrganisationId = organisationId };
        // Act
        _strategy.Calculate(request);

        // Assert is handled by ExpectedException
    }
}
