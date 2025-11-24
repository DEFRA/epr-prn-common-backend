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
    [DataRow(200, 0.60, 0.40, 120, 80, 2024)]
	[DataRow(1523, 0.65, 0.57, 990, 565, 2025)]
	[DataRow(2345, 0.73, 0.71, 1712, 1214, 2026)]
	[DataRow(1234, 0.68, 0.69, 840, 580, 2027)]
	public void Calculate_ShouldReturnCorrectObligationCalculations(int materialWeight, double glassRecyclingTarget, double remeltRecyclingTarget, int expectedGlassObligationValue, int expectedRemeltObligationValue, int submissionPeriod)
    {
		// Arrange
		var organisationId = Guid.NewGuid();
		var complianceYear = submissionPeriod + 1;

		var calculationRequest = new SubmissionCalculationRequest
        {
            SubmissionPeriod = $"{submissionPeriod}",
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
				complianceYear,
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
            RecyclingTargets = recyclingTargets,
            ComplianceYear = complianceYear
		};

        _mockCalculationService.Setup(x => x.CalculateGlass(
            request.RecyclingTargets[complianceYear][MaterialType.Glass], request.RecyclingTargets[complianceYear][MaterialType.GlassRemelt],
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
        glassCalculation.Year.Should().Be(complianceYear);

		remeltCalculation.Should().NotBeNull();
		remeltCalculation.MaterialObligationValue.Should().Be(expectedRemeltObligationValue);
		remeltCalculation.OrganisationId.Should().Be(organisationId);
		remeltCalculation.Tonnage.Should().Be(materialWeight);
		remeltCalculation.Year.Should().Be(complianceYear);
    }
}