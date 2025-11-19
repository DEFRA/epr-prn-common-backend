using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPR.PRN.Backend.API.UnitTests.Handlers.ExporterJourney
{
    [TestClass]
    public class GetAddressForServiceOfNoticesQueryHandlerTests
    {
        private Mock<IRegistrationRepository> _registrationRepositoryMock;
        private GetAddressForServiceOfNoticesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _registrationRepositoryMock = new Mock<IRegistrationRepository>();
            _handler = new GetAddressForServiceOfNoticesQueryHandler(_registrationRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRegistration_ReturnsAddress()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var registration = new Registration
            {
                ExternalId = registrationId,
                LegalDocumentAddress = new Address
                {
                    Id = 1,
                    AddressLine1 = "123 Main St",
                    TownCity = "Sample City",
                    PostCode = "12345"
                }
            };

            _registrationRepositoryMock
                .Setup(r => r.GetRegistrationByExternalId(registrationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(registration);

            var query = new GetAddressForServiceOfNoticesQuery { RegistrationId = registrationId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(registrationId, result.RegistrationId);
            Assert.IsNotNull(result.LegalDocumentAddress);
            Assert.AreEqual("123 Main St", result.LegalDocumentAddress.AddressLine1);
        }

        [TestMethod]
        public async Task Handle_RegistrationNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var registrationId = Guid.NewGuid();

            _registrationRepositoryMock
                .Setup(r => r.GetRegistrationByExternalId(registrationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Registration)null);

            var query = new GetAddressForServiceOfNoticesQuery { RegistrationId = registrationId };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_LegalDocumentAddressIdExists_FetchesAddressFromRepository()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var registration = new Registration
            {
                ExternalId = registrationId,
                LegalDocumentAddressId = 1
            };

            var legalDocumentAddress = new Address
            {
                Id = 1,
                AddressLine1 = "456 Another St",
                TownCity = "Another City",
                PostCode = "67890"
            };

            _registrationRepositoryMock
                .Setup(r => r.GetRegistrationByExternalId(registrationId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(registration);

            _registrationRepositoryMock
                .Setup(r => r.GetLegalDocumentAddress(registrationId))
                .ReturnsAsync(legalDocumentAddress);

            var query = new GetAddressForServiceOfNoticesQuery { RegistrationId = registrationId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(registrationId, result.RegistrationId);
            Assert.IsNotNull(result.LegalDocumentAddress);
            Assert.AreEqual("456 Another St", result.LegalDocumentAddress.AddressLine1);
        }
    }
}
