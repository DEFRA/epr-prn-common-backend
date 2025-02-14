using FluentAssertions;

namespace EPR.PRN.Backend.Obligation.UnitTests.Mappers;

[TestClass]
public class MaterialsMapperTests
{
    [TestMethod]
    [DataRow("Aluminium", "Aluminium")]
    [DataRow("Glass Other", "Glass")]
    [DataRow("Glass Re-melt", "GlassRemelt")]
    [DataRow("Paper/board", "Paper")]
    [DataRow("Plastic", "Plastic")]
    [DataRow("Steel", "Steel")]
    [DataRow("Wood", "Wood")]
    public void MapMaterialName_Returns_Adjusted_MaterialName(string rawMaterial, string expectedMaterial)
    {
        string actualMaterial = EPR.PRN.Backend.Obligation.Mappers.MaterialsMapper.MapMaterialName(rawMaterial);

        actualMaterial.Should().Be(expectedMaterial);
    }
}
