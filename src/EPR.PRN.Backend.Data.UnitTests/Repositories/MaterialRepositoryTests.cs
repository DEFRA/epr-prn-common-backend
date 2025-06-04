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
			new Material { Id = 1, MaterialCode = "PL", MaterialName = MaterialType.Plastic.ToString() },
			new Material { Id = 2, MaterialCode = "WD", MaterialName = MaterialType.Wood.ToString() },
			new Material { Id = 3, MaterialCode = "AL", MaterialName = MaterialType.Aluminium.ToString() },
			new Material { Id = 4, MaterialCode = "ST", MaterialName = MaterialType.Steel.ToString() },
			new Material { Id = 5, MaterialCode = "PC", MaterialName = MaterialType.Paper.ToString() },
			new Material { Id = 6, MaterialCode = "GL", MaterialName = MaterialType.Glass.ToString() },
			new Material { Id = 7, MaterialCode = "GR", MaterialName = MaterialType.GlassRemelt.ToString() },
			new Material { Id = 8, MaterialCode = "FC", MaterialName = MaterialType.FibreComposite.ToString() }
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
	public async Task GetAllMaterials_ShouldReturnAllMaterials()
	{
		// Act
		var result = await _materialRepository.GetAllMaterials();

		// Assert
		result.Should().NotBeNull(); // Check that result is not null
		result.Should().HaveCount(8); // Check that 7 materials are returned
		result.Should().Contain(material => material.MaterialCode == "PL" && material.MaterialName == MaterialType.Plastic.ToString());
		result.Should().Contain(material => material.MaterialCode == "WD" && material.MaterialName == MaterialType.Wood.ToString());
		result.Should().Contain(material => material.MaterialCode == "AL" && material.MaterialName == MaterialType.Aluminium.ToString());
		result.Should().Contain(material => material.MaterialCode == "ST" && material.MaterialName == MaterialType.Steel.ToString());
		result.Should().Contain(material => material.MaterialCode == "PC" && material.MaterialName == MaterialType.Paper.ToString());
		result.Should().Contain(material => material.MaterialCode == "GL" && material.MaterialName == MaterialType.Glass.ToString());
		result.Should().Contain(material => material.MaterialCode == "GR" && material.MaterialName == MaterialType.GlassRemelt.ToString());
		result.Should().Contain(material => material.MaterialCode == "FC" && material.MaterialName == MaterialType.FibreComposite.ToString());
	}
}
