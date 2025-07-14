using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EPR.PRN.Backend.API.Controllers.ExporterJourney;
using EPR.PRN.Backend.API.Commands.ExporterJourney;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.API.Queries.ExporterJourney;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.ExporterJourney
{
    [TestClass]
    public class AddressForServiceOfNoticesControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IValidator<UpsertAddressForServiceOfNoticesCommand>> _validatorMock;
        private AddressForServiceOfNoticesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<UpsertAddressForServiceOfNoticesCommand>>();
            _controller = new AddressForServiceOfNoticesController(_mediatorMock.Object, _validatorMock.Object);
        }

        [TestMethod]
        public async Task GetAddressForServiceOfNotices_ReturnsOkResult_WhenAddressExists()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var expectedDto = new GetAddressForServiceOfNoticesDto
            {
                RegistrationId = registrationId,
                LegalDocumentAddress = new AddressDto
                {
                    Id = 1,
                    AddressLine1 = "123 Main St",
                    TownCity = "Sample City",
                    PostCode = "12345"
                }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAddressForServiceOfNoticesQuery>(), default))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _controller.GetAddressForServiceOfNotices(registrationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(expectedDto, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateAddressForServiceOfNotices_ReturnsOkResult_WhenSuccessful()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var dto = new UpsertAddressForServiceOfNoticesDto
            {
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "123 Main St",
                    TownCity = "Sample City",
                    PostCode = "12345"
                }
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UpsertAddressForServiceOfNoticesCommand>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpsertAddressForServiceOfNoticesCommand>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAddressForServiceOfNotices(registrationId, userId, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task UpdateAddressForServiceOfNotices_ReturnsNotFound_WhenRegistrationNotFound()
        {
            // Arrange
            var registrationId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var dto = new UpsertAddressForServiceOfNoticesDto
            {
                LegalDocumentAddress = new AddressDto
                {
                    AddressLine1 = "123 Main St",
                    TownCity = "Sample City",
                    PostCode = "12345"
                }
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<UpsertAddressForServiceOfNoticesCommand>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpsertAddressForServiceOfNoticesCommand>(), default))
                .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateAddressForServiceOfNotices(registrationId, userId, dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
