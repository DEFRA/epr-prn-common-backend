using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EPR.PRN.Backend.API.Handlers.ExporterJourney;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney
{
    [TestClass]
    public class UpsertAddressForServiceOfNoticesHandlerTests
    {
        private Mock<IRegistrationRepository> _registrationRepositoryMock;
        private UpsertAddressForServiceOfNoticesHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _registrationRepositoryMock = new Mock<IRegistrationRepository>();
            _handler = new UpsertAddressForServiceOfNoticesHandler(_registrationRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidCommand_UpdatesAddressSuccessfully()
        {
            // Arrange
            var command = new UpsertAddressForServiceOfNoticesCommand
            {
                UserId = Guid.NewGuid(),
                RegistrationId = Guid.NewGuid(),
                Dto = new UpsertAddressForServiceOfNoticesDto
                {
                    LegalDocumentAddress = new AddressDto
                    {
                        AddressLine1 = "123 Main St",
                        TownCity = "Sample City",
                        PostCode = "12345"
                    }
                }
            };

            _registrationRepositoryMock
                .Setup(r => r.UpsertLegalDocumentAddress(command.RegistrationId, command.Dto.LegalDocumentAddress))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _registrationRepositoryMock.Verify(r => r.UpsertLegalDocumentAddress(command.RegistrationId, command.Dto.LegalDocumentAddress), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Handle_InvalidRegistrationId_ThrowsKeyNotFoundException()
        {
            // Arrange
            var command = new UpsertAddressForServiceOfNoticesCommand
            {
                UserId = Guid.NewGuid(),
                RegistrationId = Guid.NewGuid(),
                Dto = new UpsertAddressForServiceOfNoticesDto
                {
                    LegalDocumentAddress = new AddressDto
                    {
                        AddressLine1 = "123 Main St",
                        TownCity = "Sample City",
                        PostCode = "12345"
                    }
                }
            };

            _registrationRepositoryMock
                .Setup(r => r.UpsertLegalDocumentAddress(command.RegistrationId, command.Dto.LegalDocumentAddress))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _registrationRepositoryMock.Verify(r => r.UpsertLegalDocumentAddress(command.RegistrationId, command.Dto.LegalDocumentAddress), Times.Once);
        }
    }
}

