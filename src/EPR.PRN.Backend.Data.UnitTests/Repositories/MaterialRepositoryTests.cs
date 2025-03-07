using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories;

[TestClass]
public class MaterialRepositoryTests
{
    private MaterialRepository _materialRepository;
    private Mock<EprContext> _mockEprContext;
	private readonly List<Material> _materials =
        [
			new Material { Id = 1, MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 2, MaterialCode = "WD", MaterialName = MaterialType.Wood.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 3, MaterialCode = "AL", MaterialName = MaterialType.Aluminium.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 4, MaterialCode = "ST", MaterialName = MaterialType.Steel.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 5, MaterialCode = "PC", MaterialName = MaterialType.Paper.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 6, MaterialCode = "GL", MaterialName = MaterialType.Glass.ToString(), IsCaculable = true, IsVisibleToObligation = true },
			new Material { Id = 7, MaterialCode = "GR", MaterialName = MaterialType.GlassRemelt.ToString(), IsCaculable = false, IsVisibleToObligation = true },
			new Material { Id = 8, MaterialCode = "FC", MaterialName = MaterialType.FibreComposite.ToString(), IsCaculable = true, IsVisibleToObligation = false }
		];

    [TestInitialize]
    public void Setup()
    {
        var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
        _mockEprContext = new Mock<EprContext>(dbContextOptions);
        _mockEprContext.Setup(context => context.Material).ReturnsDbSet(_materials);
        _materialRepository = new MaterialRepository(_mockEprContext.Object);
	}

	[TestMethod]
    public async Task GetCalculableMaterials_ShouldReturnMaterials_When_IsCalculable_IsTrue()
    {
        // Act
        var result = await _materialRepository.GetCalculableMaterials();

        // Assert
        result.Should().NotBeNull(); // Check that result is not null
        result.Should().HaveCount(7); // Check that 7 materials are returned
		result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == MaterialType.Plastic.ToString());
		result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == MaterialType.Wood.ToString());
		result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == MaterialType.Aluminium.ToString());
		result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == MaterialType.Steel.ToString());
		result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == MaterialType.Paper.ToString());
		result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == MaterialType.Glass.ToString());
		result.Should().Contain(material => material.MaterialCode == "FC" && material.MaterialName == MaterialType.FibreComposite.ToString());
	}

	[TestMethod]
	public async Task GetVisibleToObligationMaterials_ShouldReturnMaterials_When_IsVisibleToObligation_IsTrue()
	{
		// Act
		var result = await _materialRepository.GetVisibleToObligationMaterials();

		// Assert
		result.Should().NotBeNull(); // Check that result is not null
		result.Should().HaveCount(7); // Check that 7 materials are returned
		result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == MaterialType.Plastic.ToString());
		result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == MaterialType.Wood.ToString());
		result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == MaterialType.Aluminium.ToString());
		result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == MaterialType.Steel.ToString());
		result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == MaterialType.Paper.ToString());
		result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == MaterialType.Glass.ToString());
		result.Should().Contain(material => material.MaterialCode == "GR" && material.MaterialName == MaterialType.GlassRemelt.ToString());
	}
}
