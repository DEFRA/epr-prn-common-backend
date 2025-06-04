using AutoFixture;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.API.Services;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Obligation.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests;

[TestClass]
public class MaterialExemptionReferenceServiceTests
{

    private readonly Mock<ILogger<MaterialExemptionReferenceService>> _loggerMock;
    private readonly Mock<IMaterialExemptionReferenceRepository> _repositoryMock;
    private readonly MaterialExemptionReferenceService _service;  
   
    public   MaterialExemptionReferenceServiceTests()
    {
        _loggerMock = new Mock<ILogger<MaterialExemptionReferenceService>>();
        _repositoryMock = new Mock<IMaterialExemptionReferenceRepository>();
        _service = new MaterialExemptionReferenceService(_loggerMock.Object, _repositoryMock.Object);
    }

    [TestMethod]
    public async Task CreateMaterialExemptionReferenceAsync_ValidData_ReturnsTrue()
    {
        // Arrange  
        var materialExemptionReferences = new List<MaterialExemptionReferenceRequest>
       {
           new MaterialExemptionReferenceRequest
           {
               ExternalId = Guid.Parse("3F9C9F5D-2D29-4C11-A0CB-77A0E02DB9C8"),
               RegistrationMaterialId = 1,
               ReferenceNumber = "ref123"
           }
       };
        _repositoryMock
            .Setup(repo => repo.CreateMaterialExemptionReference(It.IsAny<List<MaterialExemptionReference>>()))
            .ReturnsAsync(true);
        // Act  
        var result = await _service.CreateMaterialExemptionReferenceAsync(materialExemptionReferences, CancellationToken.None);
        // Assert  
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateMaterialExemptionReferenceAsync_InvalidData_ReturnsFalse()
    {
        // Arrange  
        var materialExemptionReferences = new List<MaterialExemptionReferenceRequest>
        {
            new MaterialExemptionReferenceRequest
            {
                ExternalId = Guid.Empty,
                RegistrationMaterialId = 0,
                ReferenceNumber = string.Empty
            }
        };
        _repositoryMock
            .Setup(repo => repo.CreateMaterialExemptionReference(It.IsAny<List<MaterialExemptionReference>>()))
            .ReturnsAsync(false);
        // Act  
        var result = await _service.CreateMaterialExemptionReferenceAsync(materialExemptionReferences, CancellationToken.None);
        // Assert  
        Assert.IsFalse(result);
    }
    [TestMethod]
    public async Task CreateMaterialExemptionReferenceAsync_EmptyList_ReturnsFalse()
    {
        // Arrange  
        var materialExemptionReferences = new List<MaterialExemptionReferenceRequest>();
        _repositoryMock
            .Setup(repo => repo.CreateMaterialExemptionReference(It.IsAny<List<MaterialExemptionReference>>()))
            .ReturnsAsync(false);
        // Act  
        var result = await _service.CreateMaterialExemptionReferenceAsync(materialExemptionReferences, CancellationToken.None);
        // Assert  
        Assert.IsFalse(result);
    }
    [TestMethod]
    public async Task CreateMaterialExemptionReferenceAsync_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var requests = new List<MaterialExemptionReferenceRequest>
        {
            new MaterialExemptionReferenceRequest
            {
                ExternalId = Guid.NewGuid(),
                RegistrationMaterialId = 0,
                ReferenceNumber = "REF001"
            }
        };
        _repositoryMock
            .Setup(r => r.CreateMaterialExemptionReference(It.IsAny<List<MaterialExemptionReference>>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateMaterialExemptionReferenceAsync(requests, CancellationToken.None);

        // Assert
        Assert.IsFalse(result); 
    }

}
