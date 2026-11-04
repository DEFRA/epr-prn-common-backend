using AwesomeAssertions;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using EPR.PRN.Backend.Obligation.Services;
using EPR.PRN.Backend.Obligation.Strategies;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EPR.PRN.Backend.Obligation.UnitTests.Services;

// Portable examples at CalculateAsync: repositories supply reference data; the resolver,
// strategies and arithmetic are real. Weight is an int, targets are doubles, days nullable int.
[TestClass]
public class CalculationParityTests
{
    private static readonly Guid Producer = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid Submitter = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Material[] Materials =
    [
        new() { Id = 1, MaterialCode = "PL", MaterialName = "Plastic" },
        new() { Id = 2, MaterialCode = "PC", MaterialName = "Paper" },
        new() { Id = 3, MaterialCode = "FC", MaterialName = "FibreComposite" },
        new() { Id = 4, MaterialCode = "GL", MaterialName = "Glass" },
        new() { Id = 5, MaterialCode = "GR", MaterialName = "GlassRemelt" },
        new() { Id = 6, MaterialCode = "AL", MaterialName = "Aluminium" },
        new() { Id = 7, MaterialCode = "ST", MaterialName = "Steel" },
        new() { Id = 8, MaterialCode = "WD", MaterialName = "Wood" }
    ];

    // Seeded by RecreateRecyclingTargetTable and AddFiberCompositetothelistofmaterialsandrecyclingtargets.
    // Database decimals become doubles at this boundary; each row is an obligation year.
    private static Dictionary<int, Dictionary<MaterialType, double>> Targets() => new()
    {
        [2025] = YearTargets(0.75, 0.74, 0.61, 0.80, 0.55, 0.45, 0.75, 0.75),
        [2026] = YearTargets(0.77, 0.76, 0.62, 0.81, 0.57, 0.46, 0.76, 0.77),
        [2027] = YearTargets(0.79, 0.78, 0.63, 0.82, 0.59, 0.47, 0.77, 0.79),
        [2028] = YearTargets(0.81, 0.80, 0.64, 0.83, 0.61, 0.48, 0.78, 0.81),
        [2029] = YearTargets(0.83, 0.82, 0.65, 0.84, 0.63, 0.49, 0.79, 0.83),
        [2030] = YearTargets(0.85, 0.85, 0.67, 0.85, 0.65, 0.50, 0.80, 0.85)
    };

    private static Dictionary<MaterialType, double> YearTargets(double paper, double glass, double aluminium,
        double steel, double plastic, double wood, double remelt, double fibreComposite) => new()
    {
        [MaterialType.Paper] = paper, [MaterialType.Glass] = glass, [MaterialType.Aluminium] = aluminium,
        [MaterialType.Steel] = steel, [MaterialType.Plastic] = plastic, [MaterialType.Wood] = wood,
        [MaterialType.GlassRemelt] = remelt, [MaterialType.FibreComposite] = fibreComposite
    };

    private static ObligationCalculatorService CreateService(Dictionary<int, Dictionary<MaterialType, double>> targets = null, IEnumerable<Material> materials = null)
    {
        var targetRepository = new Mock<IRecyclingTargetDataService>();
        targetRepository.Setup(x => x.GetRecyclingTargetsAsync()).ReturnsAsync(targets ?? Targets());
        var materialRepository = new Mock<IMaterialRepository>();
        materialRepository.Setup(x => x.GetAllMaterials()).ReturnsAsync(materials ?? Materials);
        var submitterTypes = new Mock<IObligationCalculationOrganisationSubmitterTypeRepository>();
        submitterTypes.Setup(x => x.GetSubmitterTypeIdByTypeName(It.IsAny<ObligationCalculationOrganisationSubmitterTypeName>()))
            .ReturnsAsync((ObligationCalculationOrganisationSubmitterTypeName type) => (int)type);
        var arithmetic = new MaterialCalculationService();
        var resolver = new MaterialCalculationStrategyResolver(
            [new GeneralCalculationStrategy(arithmetic), new GlassCalculationStrategy(arithmetic)],
            NullLogger<MaterialCalculationStrategyResolver>.Instance);
        return new ObligationCalculatorService(Mock.Of<IObligationCalculationRepository>(), targetRepository.Object,
            resolver, NullLogger<ObligationCalculatorService>.Instance, Mock.Of<IPrnRepository>(),
            materialRepository.Object, submitterTypes.Object);
    }

    private static SubmissionCalculationRequest Row(string material, int weight, string period = "2024", int? days = null,
        string submitterType = "ComplianceScheme") => new()
    {
        OrganisationId = Producer, SubmitterId = Submitter, SubmitterType = submitterType,
        SubmissionPeriod = period, PackagingMaterial = material, PackagingMaterialWeight = weight,
        NumberOfDaysObligated = days
    };

    // 101 × .55 rounds up to 56 before the partial-year scale: 56 × 182/365 rounds to 28.
    [TestMethod]
    [DataRow("PL", 101, "2024", null, 56)]
    [DataRow("PL", 101, "2024", 182, 28)]
    [DataRow("PL", 101, "2027", 183, 31)]
    [DataRow("PL", 101, "2024", 0, 0)]
    [DataRow("FC", 101, "2024", null, 76)]
    [DataRow("PC", 100, "2024", null, 75)]
    [DataRow("AL", 100, "2024", null, 61)]
    [DataRow("ST", 100, "2024", null, 80)]
    [DataRow("WD", 100, "2024", null, 45)]
    public async Task General_examples(string material, int weight, string period, int? days, int expected)
    {
        var row = Row(material, weight, period, days);
        var before = DateTime.UtcNow;
        var result = await CreateService().CalculateAsync(Guid.NewGuid(), [row]);
        var after = DateTime.UtcNow;
        result.Success.Should().BeTrue();
        var calculation = result.Calculations.Should().ContainSingle().Subject;
        calculation.MaterialId.Should().Be(Materials.Single(m => m.MaterialCode == material).Id);
        calculation.MaterialObligationValue.Should().Be(expected);
        calculation.Year.Should().Be(int.Parse(period) + 1);
        calculation.Tonnage.Should().Be(weight);
        calculation.OrganisationId.Should().Be(Producer);
        calculation.SubmitterId.Should().Be(Submitter);
        calculation.SubmitterTypeId.Should().Be(1);
        calculation.CalculatedOn.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
        calculation.IsDeleted.Should().BeFalse();
    }

    // 101 × .74 = 74.74; remelt ceil(74.74 × .75) = 57, remainder 75 − 57 = 18.
    [TestMethod]
    [DataRow("2024", null, 18, 57)]
    [DataRow("2024", 182, 9, 29)]
    [DataRow("2027", 183, 9, 32)]
    [DataRow("2024", 0, 0, 0)]
    public async Task Glass_examples(string period, int? days, int remainder, int remelt)
    {
        var row = Row("GL", 101, period, days, "DirectRegistrant");
        row.OrganisationId = Submitter;
        var result = await CreateService().CalculateAsync(Submitter, [row]);
        result.Success.Should().BeTrue();
        result.Calculations.Should().HaveCount(2);
        result.Calculations.Select(x => (x.MaterialId, x.MaterialObligationValue)).Should()
            .Equal((4, remainder), (5, remelt));
        result.Calculations.Should().OnlyContain(x => x.OrganisationId == Submitter && x.SubmitterId == Submitter &&
            x.SubmitterTypeId == 2 && x.Year == int.Parse(period) + 1 && x.Tonnage == 101 && !x.IsDeleted);
        result.Calculations[0].CalculatedOn.Should().Be(result.Calculations[1].CalculatedOn);
    }

    // Obligation year 2025, PL target 0.55, weight 2, 183 days.
    // ceil(2 × 0.55) = 2, then ceil(2 × 183/365) = 2; scaling before rounding gives 1.
    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task Same_partial_year_rounding_result_for_both_submitter_types(string submitterType)
    {
        var row = Row("PL", 2, "2024", 183, submitterType);
        row.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService().CalculateAsync(Submitter, [row]);
        result.Success.Should().BeTrue();
        var record = result.Calculations.Should().ContainSingle().Subject;
        (record.OrganisationId, record.SubmitterId, record.SubmitterTypeId, record.MaterialId,
            record.Year, record.Tonnage, record.MaterialObligationValue).Should()
            .Be((row.OrganisationId, Submitter, submitterType == "DirectRegistrant" ? 2 : 1, 1, 2025, 2, 2));
    }

    // 2028 is a leap year: ceil(610 × 365/366) = 609, not 610.
    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task Same_leap_year_result_for_both_submitter_types(string submitterType)
    {
        var row = Row("PL", 1000, "2027", 365, submitterType);
        row.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService().CalculateAsync(Submitter, [row]);
        result.Success.Should().BeTrue();
        var record = result.Calculations.Should().ContainSingle().Subject;
        (record.Year, record.Tonnage, record.MaterialObligationValue).Should().Be((2028, 1000, 609));
    }

    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task Same_target_year_results_for_both_submitter_types(string submitterType)
    {
        // Identical PL weights, seeded targets: 2025 = 0.55, 2026 = 0.57.
        var targets = Targets();
        var first = Row("PL", 100, "2024", submitterType: submitterType);
        var second = Row("PL", 100, "2025", submitterType: submitterType);
        first.OrganisationId = second.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService(targets).CalculateAsync(Submitter, [first, second]);
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.Year, x.MaterialObligationValue)).Should().Equal((2025, 55), (2026, 57));
    }

    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task General_rounding_at_and_above_an_integer_for_both_submitter_types(string submitterType)
    {
        // PL target 0.55: 20 tonnes produces exactly 11; 21 produces 11.55, rounded to 12.
        var exact = Row("PL", 20, submitterType: submitterType);
        var fractional = Row("PL", 21, submitterType: submitterType);
        exact.OrganisationId = fractional.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService().CalculateAsync(Submitter, [exact, fractional]);
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.Tonnage, x.MaterialObligationValue)).Should().Equal((20, 11), (21, 12));
    }

    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task Glass_remelt_uses_unrounded_glass_target_for_both_submitter_types(string submitterType)
    {
        // 3 tonnes × 0.74 = 2.22; remelt ceil(2.22 × 0.75) = 2.
        // Total ceil(2.22) = 3, leaving 1. Rounding total before remelt would give 3 and 0.
        var row = Row("GL", 3, submitterType: submitterType);
        row.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService().CalculateAsync(Submitter, [row]);
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.MaterialId, x.Tonnage, x.MaterialObligationValue))
            .Should().Equal((4, 3, 1), (5, 3, 2));
    }

    [TestMethod]
    [DataRow("DirectRegistrant")]
    [DataRow("ComplianceScheme")]
    public async Task Glass_components_are_rounded_separately_for_both_submitter_types(string submitterType)
    {
        // 101 tonnes at glass 0.74 and remelt 0.75 gives 18 remainder and 57 remelt.
        // With one day of 365, each positive component rounds up to 1; rounding their sum gives 1 instead of 2.
        var row = Row("GL", 101, days: 1, submitterType: submitterType);
        row.OrganisationId = submitterType == "DirectRegistrant" ? Submitter : Producer;
        var result = await CreateService().CalculateAsync(Submitter, [row]);
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.MaterialId, x.Tonnage, x.MaterialObligationValue))
            .Should().Equal((4, 101, 1), (5, 101, 1));
    }

    [TestMethod]
    public async Task Compliance_scheme_calculates_each_member_and_preserves_identity()
    {
        // Submission 2024, obligation year 2025. Targets: PL 0.55, PC 0.75,
        // GL 0.74 and remelt 0.75. A: PL 2 for 183 days. B: PC 100 and GL 101.
        var memberB = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var plastic = Row("PL", 2, days: 183);
        var paper = Row("PC", 100); paper.OrganisationId = memberB;
        var glass = Row("GL", 101); glass.OrganisationId = memberB;
        var before = DateTime.UtcNow;
        var result = await CreateService().CalculateAsync(Submitter, [plastic, paper, glass]);
        var after = DateTime.UtcNow;
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.OrganisationId, x.SubmitterId, x.SubmitterTypeId,
            x.MaterialId, x.Year, x.Tonnage, x.MaterialObligationValue)).Should().Equal(
                (Producer, Submitter, 1, 1, 2025, 2, 2),
                (memberB, Submitter, 1, 2, 2025, 100, 75),
                (memberB, Submitter, 1, 4, 2025, 101, 18),
                (memberB, Submitter, 1, 5, 2025, 101, 57));
        result.Calculations.Should().OnlyContain(x => !x.IsDeleted && x.CalculatedOn >= before && x.CalculatedOn <= after);
    }

    // Each seeded year uses 100 tonnes of each input material. Glass produces a
    // remainder and remelt record from the two glass targets.
    [TestMethod]
    [DataRow(2025, 55, 75, 75, 61, 80, 45, 18, 56)]
    [DataRow(2026, 57, 77, 77, 62, 81, 46, 18, 58)]
    [DataRow(2027, 59, 79, 79, 63, 82, 47, 17, 61)]
    [DataRow(2028, 61, 81, 81, 64, 83, 48, 17, 63)]
    [DataRow(2029, 63, 83, 83, 65, 84, 49, 17, 65)]
    [DataRow(2030, 65, 85, 85, 67, 85, 50, 17, 68)]
    public async Task Seeded_targets_produce_expected_records_for_each_year(int year, int plastic, int paper,
        int fibre, int aluminium, int steel, int wood, int glass, int remelt)
    {
        var rows = new[] { "PL", "PC", "FC", "AL", "ST", "WD", "GL" }
            .Select(code => Row(code, 100, (year - 1).ToString())).ToList();
        var result = await CreateService().CalculateAsync(Submitter, rows);
        result.Success.Should().BeTrue();
        result.Calculations.Select(x => (x.MaterialId, x.Year, x.Tonnage, x.MaterialObligationValue)).Should().Equal(
            (1, year, 100, plastic), (2, year, 100, paper), (3, year, 100, fibre),
            (6, year, 100, aluminium), (7, year, 100, steel), (8, year, 100, wood),
            (4, year, 100, glass), (5, year, 100, remelt));
    }

    [TestMethod]
    public async Task Missing_year_or_glass_targets_throw_instead_of_skipping()
    {
        var missingYear = Targets();
        missingYear.Remove(2025);
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => CreateService(missingYear).CalculateAsync(Submitter, [Row("PL", 100)]));
        foreach (var material in new[] { MaterialType.Glass, MaterialType.GlassRemelt })
        {
            var missingTarget = Targets();
            missingTarget[2025].Remove(material);
            await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => CreateService(missingTarget).CalculateAsync(Submitter, [Row("GL", 100)]));
        }
    }

    [TestMethod]
    public async Task General_material_name_validation_is_case_insensitive_but_reference_lookup_is_not()
    {
        var materials = Materials.Select(m => m.MaterialCode == "PL"
            ? new Material { Id = m.Id, MaterialCode = m.MaterialCode, MaterialName = "plastic" } : m);
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => CreateService(materials: materials).CalculateAsync(Submitter, [Row("PL", 100)]));
    }

    [TestMethod]
    public async Task Invalid_rows_are_skipped_when_another_row_succeeds()
    {
        var invalidPeriod = Row("PL", 100, "invalid");
        var invalidMaterial = Row("missing", 100);
        var unsupportedRemelt = Row("GR", 100);
        var result = await CreateService().CalculateAsync(Submitter, [invalidPeriod, invalidMaterial, unsupportedRemelt, Row("PL", 100)]);
        result.Success.Should().BeTrue();
        result.Calculations.Should().ContainSingle().Which.MaterialObligationValue.Should().Be(55);
        var noValidRows = await CreateService().CalculateAsync(Submitter, [invalidPeriod, invalidMaterial, unsupportedRemelt]);
        noValidRows.Success.Should().BeFalse();
        noValidRows.Calculations.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Submitter_type_is_parsed_from_first_row_and_used_for_batch()
    {
        var first = Row("PL", 100, submitterType: "ComplianceScheme");
        var second = Row("PC", 100, submitterType: "DirectRegistrant");
        var result = await CreateService().CalculateAsync(Submitter, [first, second]);
        result.Calculations.Should().HaveCount(2).And.OnlyContain(x => x.SubmitterTypeId == 1);
        first.SubmitterType = null;
        var invalid = await CreateService().CalculateAsync(Submitter, [first, second]);
        invalid.Success.Should().BeFalse();
        invalid.Calculations.Should().BeEmpty();
    }

    [TestMethod]
    public async Task Missing_or_invalid_submitter_type_fails_without_calculations()
    {
        foreach (var type in new[] { null, "", "unknown" })
        {
            var result = await CreateService().CalculateAsync(Submitter, [Row("PL", 100, submitterType: type)]);
            result.Success.Should().BeFalse();
            result.Calculations.Should().BeEmpty();
        }
    }

    [TestMethod]
    public async Task Empty_material_codes_and_unrecognised_reference_names_are_skipped()
    {
        var materials = Materials.Concat([new Material { Id = 9, MaterialCode = "XX", MaterialName = "Unknown" }]);
        var result = await CreateService(materials: materials).CalculateAsync(Submitter,
            [Row(null, 100), Row("", 100), Row("XX", 100), Row("PL", 100)]);
        result.Success.Should().BeTrue();
        result.Calculations.Should().ContainSingle().Which.MaterialObligationValue.Should().Be(55);
    }

    [TestMethod]
    public async Task Large_weight_and_overflow_are_observable_at_the_calculation_boundary()
    {
        var result = await CreateService().CalculateAsync(Submitter, [Row("ST", int.MaxValue)]);
        result.Calculations.Should().ContainSingle().Which.MaterialObligationValue.Should().Be(1717986918);
        var targets = Targets();
        // Deliberately synthetic target to characterise overflow; not a database seed.
        targets[2025][MaterialType.Steel] = 2.0;
        await Assert.ThrowsExactlyAsync<OverflowException>(() => CreateService(targets)
            .CalculateAsync(Submitter, [Row("ST", int.MaxValue)]));
    }

    [TestMethod]
    public async Task Missing_target_and_missing_glass_remelt_reference_throw()
    {
        var targets = Targets();
        targets[2025].Remove(MaterialType.Plastic);
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => CreateService(targets).CalculateAsync(Submitter, [Row("PL", 100)]));
        await Assert.ThrowsExactlyAsync<KeyNotFoundException>(() => CreateService(materials: Materials.Where(m => m.MaterialCode != "GR"))
            .CalculateAsync(Submitter, [Row("GL", 100)]));
    }
}
