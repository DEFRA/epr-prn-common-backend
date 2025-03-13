using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Services;
using EPR.PRN.Backend.Obligation.Strategies;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies;

[TestClass]
public class GeneralCalculationStrategyTests
{
    private Mock<IMaterialCalculationService> _mockCalculationService;
	private Mock<IDateTimeProvider> _mockDateTimeProvider;
	private GeneralCalculationStrategy _strategy;

    [TestInitialize]
    public void SetUp()
    {
        _mockCalculationService = new Mock<IMaterialCalculationService>();
		_mockDateTimeProvider = new Mock<IDateTimeProvider>();
		_strategy = new GeneralCalculationStrategy(_mockCalculationService.Object, _mockDateTimeProvider.Object);
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
        result.Should().BeTrue();
    }

    [TestMethod]
    [DataRow(MaterialType.Glass)]
    [DataRow(MaterialType.GlassRemelt)]
    public void CanHandle_ShouldReturnFalse_ForNonGeneralMaterials(MaterialType materialType)
    {
        // Act
        var result = _strategy.CanHandle(materialType);

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
	[DataRow(100, 0.70, 70, 0)]
	[DataRow(101, 0.70, 71, 0)]
    [DataRow(101, 0.71, 72, 1)]
	[DataRow(101, 0.72, 73, 2)]
	[DataRow(101, 0.73, 74, 3)]
	[DataRow(101, 0.74, 75, 4)]
	public void Calculate_ShouldReturnCorrectObligationCalculation(int materialWeight, double recyclingTarget, int expectedRoundedMaterialObligationValue, int yearOffSet)
    {
        // Arrange
        var organisationId = Guid.NewGuid();
		var currentYear = DateTime.UtcNow.Year + yearOffSet;
		var calculatedOn = DateTime.UtcNow.AddYears(yearOffSet);
		_mockDateTimeProvider.Setup(m => m.UtcNow).Returns(calculatedOn);
		_mockDateTimeProvider.Setup(m => m.CurrentYear).Returns(currentYear);
		var submissionPeriod = $"{currentYear - 1}";

		var calculationRequest = new SubmissionCalculationRequest
        {
            PackagingMaterial = "PL",
            PackagingMaterialWeight = materialWeight,
            OrganisationId = organisationId,
            SubmissionPeriod = submissionPeriod
		};

        var materialType = MaterialType.Plastic;
        var material = new Material
		{
			Id = 1,
			MaterialName = materialType.ToString(),
			MaterialCode = "PL"
		};

		var recyclingTargets = new Dictionary<int, Dictionary<MaterialType, double>>
        {
            {
                currentYear,
                new Dictionary<MaterialType, double>
                {
                    { materialType, recyclingTarget }
                }
            }
        };

        var request = new CalculationRequestDto
        {
            SubmissionCalculationRequest = calculationRequest,
            RecyclingTargets = recyclingTargets,
            MaterialType = materialType,
            Materials = [material],
            OrganisationId = organisationId,
        };

        _strategy = new GeneralCalculationStrategy(new MaterialCalculationService(), _mockDateTimeProvider.Object);

        // Act
        var result = _strategy.Calculate(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
		result[0].MaterialId.Should().Be(1);
		result[0].MaterialObligationValue.Should().Be(expectedRoundedMaterialObligationValue);
        result[0].OrganisationId.Should().Be(organisationId);
        result[0].Year.Should().Be(currentYear);
        result[0].Tonnage.Should().Be(materialWeight);
		result[0].CalculatedOn.Should().Be(calculatedOn);
    }

    [TestMethod]
    [ExpectedException(typeof(KeyNotFoundException))]
    public void Calculate_ShouldThrowKeyNotFoundException_WhenRecyclingTargetYearNotFound()
    {
        // Arrange
        var organisationId = Guid.NewGuid();
        var calculationRequest = new SubmissionCalculationRequest
        {
            PackagingMaterial = "PL",
            PackagingMaterialWeight = 100,
            OrganisationId = Guid.NewGuid(),
            SubmissionPeriod = "2025-P1"
        };

        var materialType = MaterialType.Plastic;
		var material = new Material
		{
			Id = 1,
			MaterialName = materialType.ToString(),
			MaterialCode = "PL"
		};

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
        var request = new CalculationRequestDto
        {
            SubmissionCalculationRequest = calculationRequest,
            RecyclingTargets = recyclingTargets,
            MaterialType = materialType,
			Materials = [material],
			OrganisationId = organisationId
        };
        // Act
        _strategy.Calculate(request);

        // Assert is handled by ExpectedException
    }
}
