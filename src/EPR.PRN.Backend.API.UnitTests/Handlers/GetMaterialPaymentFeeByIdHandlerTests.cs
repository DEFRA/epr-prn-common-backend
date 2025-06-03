using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class GetMaterialPaymentFeeByIdHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private GetMaterialPaymentInfoByIdHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetMaterialPaymentInfoByIdHandler(_rmRepositoryMock.Object, _mapperMock.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var materialId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9");
        var registrationMaterial = new RegistrationMaterial { ExternalId = materialId };
        var mappedDto = new MaterialPaymentFeeDto(); // empty or with default values

        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(materialId))
                         .ReturnsAsync(registrationMaterial);

        _mapperMock.Setup(m => m.Map<MaterialPaymentFeeDto>(registrationMaterial))
                   .Returns(mappedDto);

        var query = new GetMaterialPaymentFeeByIdQuery { Id = materialId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(mappedDto);
        _mapperMock.Verify(m => m.Map<MaterialPaymentFeeDto>(registrationMaterial), Times.Once);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnDefaultDto_WhenMaterialDoesNotExist()
    {
        // Arrange
        var materialId = Guid.Parse("10E3046C-0497-4148-A32D-03DBE78E6EB1"); // Non-existent
        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(materialId))
                         .ReturnsAsync((RegistrationMaterial)null);

        var query = new GetMaterialPaymentFeeByIdQuery { Id = materialId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<MaterialPaymentFeeDto>();
        _mapperMock.Verify(m => m.Map<MaterialPaymentFeeDto>(It.IsAny<RegistrationMaterial>()), Times.Never);
    }

    [TestMethod]
    public async Task Handle_ShouldNotCallMapper_WhenMaterialIsNull()
    {
        // Arrange
        var materialId = Guid.Parse("db22649f-bcca-4187-9b95-0bc2a7159017");
        _rmRepositoryMock.Setup(r => r.GetRegistrationMaterialById(materialId))
                         .ReturnsAsync((RegistrationMaterial)null);

        var query = new GetMaterialPaymentFeeByIdQuery { Id = materialId };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map<MaterialPaymentFeeDto>(It.IsAny<RegistrationMaterial>()), Times.Never);
        result.Should().BeOfType<MaterialPaymentFeeDto>();
    }
}