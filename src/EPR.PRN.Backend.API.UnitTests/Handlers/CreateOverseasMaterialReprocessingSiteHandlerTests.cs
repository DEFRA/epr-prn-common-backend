using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DTO;
using EPR.PRN.Backend.Data.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class CreateOverseasMaterialReprocessingSiteHandlerTests
    {
        private Mock<IMaterialRepository> _repositoryMock = null!;
        private CreateOverseasMaterialReprocessingSiteHandler _handler = null!;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IMaterialRepository>();
            _handler = new CreateOverseasMaterialReprocessingSiteHandler(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldCallRepositorySaveOverseasReprocessingSites_Once_WithCorrectParameter()
        {
            // Arrange
            var updateDto = new UpdateOverseasAddressDto();
            var command = new CreateOverseasMaterialReprocessingSiteCommand
            {
                UpdateOverseasAddress = updateDto
            };

            _repositoryMock
                .Setup(r => r.SaveOverseasReprocessingSites(updateDto))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.SaveOverseasReprocessingSites(updateDto), Times.Once);
        }
    }
}
