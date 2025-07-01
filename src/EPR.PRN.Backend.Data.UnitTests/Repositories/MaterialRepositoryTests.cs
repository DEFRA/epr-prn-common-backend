using System.Xml.XPath;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Testing.Platform.Extensions;
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

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldCreateNewContact_WhenNoneExists()
    {
        // Arrange
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var registrationMaterials = new List<RegistrationMaterial>
        {
            new() { Id = registrationMaterialId, ExternalId = registrationMaterialExternalId, RegistrationMaterialContact = null }
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);

        _mockEprContext.Setup(c => c.RegistrationMaterialContacts)
            .ReturnsDbSet(new List<RegistrationMaterialContact>());

        // Act
        var result = await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationMaterialId.Should().Be(registrationMaterialId);
        
        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldUpdateExistingContact_WhenExists()
    {
        var registrationMaterialId = 5;
        var registrationMaterialExternalId = Guid.NewGuid();
        var registrationMaterialContactId = 100;
        var userId = Guid.NewGuid();

        var registrationMaterialContact = new RegistrationMaterialContact
        {
            Id = registrationMaterialContactId,
            ExternalId = Guid.NewGuid(),
            RegistrationMaterialId = registrationMaterialId,
            UserId = Guid.NewGuid()
        };

        var registrationMaterials = new List<RegistrationMaterial>
        {
            new() { Id = registrationMaterialId, ExternalId = registrationMaterialExternalId, RegistrationMaterialContact = registrationMaterialContact }
        };

        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(registrationMaterials);

        _mockEprContext.Setup(c => c.RegistrationMaterialContacts)
            .ReturnsDbSet(new List<RegistrationMaterialContact> { registrationMaterialContact});

        // Act
        var result = await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);

        // Assert
        result.Should().NotBeNull();
        result.RegistrationMaterialId.Should().Be(registrationMaterialId);
        result.UserId.Should().Be(userId);

        _mockEprContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UpsertRegistrationMaterialContact_ShouldThrow_WhenRegistrationMaterialNotFound()
    {
        // Arrange
        var registrationMaterialExternalId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _mockEprContext.Setup(c => c.RegistrationMaterials)
            .ReturnsDbSet(new List<RegistrationMaterial>());
        
        // Act & Assert
        await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
        {
            await _materialRepository.UpsertRegistrationMaterialContact(registrationMaterialExternalId, userId);
        });
    }
}