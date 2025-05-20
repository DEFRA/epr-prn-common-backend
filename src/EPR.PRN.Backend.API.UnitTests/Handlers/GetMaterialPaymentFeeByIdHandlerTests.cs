using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
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
            var materialId = 1;
            var registrationMaterial = new RegistrationMaterial { Id = materialId };
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
            var materialId = 999; // Non-existent
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
            var materialId = 42;
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
}
