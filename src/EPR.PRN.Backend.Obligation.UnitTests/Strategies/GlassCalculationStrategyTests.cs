using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Obligation.Dto;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Strategies;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Strategies;

[TestClass]
public class GlassCalculationStrategyTests
{
    private Mock<IMaterialCalculationService> _mockCalculationService;
    private Mock<IDateTimeProvider> _mockDateTimeProvider;
	private GlassCalculationStrategy _strategy;

    [TestInitialize]
    public void SetUp()
    {
        _mockCalculationService = new Mock<IMaterialCalculationService>();
		_mockDateTimeProvider = new Mock<IDateTimeProvider>();
		_strategy = new GlassCalculationStrategy(_mockCalculationService.Object, _mockDateTimeProvider.Object);
    }

    [TestMethod]
    public void CanHandle_ShouldReturnTrue_ForGlassMaterial()
    {
        // Act
        var result = _strategy.CanHandle(MaterialType.Glass);

        // Assert
        result.Should().BeTrue();
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
        result.Should().BeFalse();
    }

    [TestMethod]
    [DataRow(200, 0.60, 0.40, 120, 80, 0)]
	[DataRow(1523, 0.65, 0.57, 990, 565, 1)]
	[DataRow(2345, 0.73, 0.71, 1712, 1214, 2 )]
	[DataRow(1234, 0.68, 0.69, 840, 580, 3)]
	public void Calculate_ShouldReturnCorrectObligationCalculations(int materialWeight, double glassRecyclingTarget, double remeltRecyclingTarget, int expectedGlassObligationValue, int expectedRemeltObligationValue, int yearOffSet)
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
            SubmissionPeriod = submissionPeriod,
            PackagingMaterial = "GL",
            PackagingMaterialWeight = materialWeight,
            OrganisationId = organisationId,
            SubmitterId = organisationId,
            SubmitterType = ObligationCalculationOrganisationSubmitterTypeName.DirectRegistrant.ToString()
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
                currentYear,
                new Dictionary<MaterialType, double>
                {
                    { MaterialType.Glass, glassRecyclingTarget },
                    { MaterialType.GlassRemelt, remeltRecyclingTarget }
                }
            }
        };

        var request = new CalculationRequestDto
        {
            MaterialType = MaterialType.Glass,
			Materials = materials,
			SubmissionCalculationRequest = calculationRequest,
            SubmitterId = organisationId,
            RecyclingTargets = recyclingTargets
        };

        _mockCalculationService.Setup(x => x.CalculateGlass(
            request.RecyclingTargets[currentYear][MaterialType.Glass], request.RecyclingTargets[currentYear][MaterialType.GlassRemelt],
            request.SubmissionCalculationRequest.PackagingMaterialWeight))
            .Returns((expectedRemeltObligationValue, expectedGlassObligationValue));  // remelt = 80, remainder = 120

        // Act
        var result = _strategy.Calculate(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var glassCalculation = result.Find(x => x.MaterialId == 6);
        var remeltCalculation = result.Find(x => x.MaterialId == 7);

        glassCalculation.Should().NotBeNull();
        glassCalculation.MaterialObligationValue.Should().Be(expectedGlassObligationValue);
        glassCalculation.OrganisationId.Should().Be(organisationId);
        glassCalculation.Tonnage.Should().Be(materialWeight);
        glassCalculation.Year.Should().Be(currentYear);
        glassCalculation.CalculatedOn.Should().Be(calculatedOn);

		remeltCalculation.Should().NotBeNull();
		remeltCalculation.MaterialObligationValue.Should().Be(expectedRemeltObligationValue);
		remeltCalculation.OrganisationId.Should().Be(organisationId);
		remeltCalculation.Tonnage.Should().Be(materialWeight);
		remeltCalculation.Year.Should().Be(currentYear);
		remeltCalculation.CalculatedOn.Should().Be(calculatedOn);
    }
}