using Microsoft.VisualStudio.TestTools.UnitTesting;
using EPR.Accreditation.API.Common.Dtos;
using EPR.Accreditation.API.Controllers;
using EPR.Accreditation.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EPR.Accreditation.API.Helpers;
using Microsoft.AspNetCore.Http;

namespace EPR.Accreditation.API.UnitTests.Controllers
{
    [TestClass]
    public class PrnControllerTests
    {
        private PackageRecyclingNoteController PrnController { get; set; }
        private Mock<IPackageRecyclingNoteService> MockPrnService { get; set; }

        [TestInitialize]
        public void Init()
        {
            MockPrnService = new Mock<IPackageRecyclingNoteService>();
            PrnController = new PackageRecyclingNoteController(MockPrnService.Object);
        }

        [TestMethod]
        public async Task CreateNewPrn()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var request = new PackageRecyclingNoteRequest();

            this.MockPrnService
                .Setup(service => service.CreatePackageRecyclingNote(request))
                .ReturnsAsync(expectedId);

            // Act
            var result = (OkObjectResult)await this.PrnController.CreatePackageRecyclingNote(request);

            // Assert
            Assert.AreEqual(expectedId, result.Value);
        }

        [TestMethod]
        public async Task CreateNewPrn_MiscError()
        {
            // Arrrange
            var request = new PackageRecyclingNoteRequest();

            this.MockPrnService
                .Setup(service => service.CreatePackageRecyclingNote(request))
                .Throws<Exception>();

            // Act
            var result = (StatusCodeResult)await this.PrnController.CreatePackageRecyclingNote(request);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public async Task GetPrn_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedPrn = new PackageRecyclingNoteResponse
            {
                Note = "Test Prn",
            };

            this.MockPrnService
                .Setup(service => service.GetPackageRecyclingNote(id))
                .ReturnsAsync(expectedPrn);

            // Act
            var result = await this.PrnController.GetPackageRecyclingNote(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedPrn, ((OkObjectResult)result).Value);
            MockPrnService.Verify(s => s.GetPackageRecyclingNote(id), Times.Once());
        }

        [TestMethod]
        public async Task GetPrn_PrnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.GetPackageRecyclingNote(id))
                .ReturnsAsync(default(PackageRecyclingNoteResponse));

            // Act
            var result = await this.PrnController.GetPackageRecyclingNote(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetPrn_MiscError()
        {
            // Arrange
            var id = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.GetPackageRecyclingNote(id))
                .Throws<Exception>();

            // Act
            var result = await this.PrnController.GetPackageRecyclingNote(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCode.StatusCode);
        }

        [TestMethod]
        public async Task GetPrnsForOrganisation_Success()
        {
            // Arrange
            var organisationId = Guid.NewGuid();
            var expectedPrns = new[]
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
            };

            this.MockPrnService
                .Setup(service => service.GetPrnsForOrganisation(organisationId))
                .ReturnsAsync(expectedPrns);

            // Act
            var result = await this.PrnController.GetPrnsForOrganisation(organisationId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedPrns, ((OkObjectResult)result).Value);
            MockPrnService.Verify(s => s.GetPrnsForOrganisation(organisationId), Times.Once());
        }

        [TestMethod]
        public async Task GetPrnsForOrganisation_OrgNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.GetPrnsForOrganisation(id))
                .ReturnsAsync(new List<Guid>());

            // Act
            var result = await this.PrnController.GetPrnsForOrganisation(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetPrnsForOrganisation_MiscError()
        {
            // Arrange
            var id = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.GetPrnsForOrganisation(id))
                .Throws<Exception>();

            // Act
            var result = await this.PrnController.GetPrnsForOrganisation(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCode.StatusCode);
        }

        [TestMethod]
        public async Task UpdatePrnStatus_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var statusUpdate = new PrnStatusHistoryRequest
            {
                PrnStatusId = 2,
                Comment = "Test Status Update",
            };

            // Act
            var result = await this.PrnController.UpdatePrnStatus(id, statusUpdate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            MockPrnService.Verify(s => s.UpdatePrnStatus(id, statusUpdate), Times.Once());
        }

        [TestMethod]
        public async Task UpdatePrnStatus_PrnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var statusUpdate = new PrnStatusHistoryRequest
            {
                PrnStatusId = 2,
                Comment = "Test Status Update",
            };
            this.MockPrnService
                .Setup(service => service.UpdatePrnStatus(id, statusUpdate))
                .Throws<NotFoundException>();

            // Act
            var result = await this.PrnController.UpdatePrnStatus(id, statusUpdate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePrnStatus_MiscError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var statusUpdate = new PrnStatusHistoryRequest
            {
                PrnStatusId = 2,
                Comment = "Test Status Update",
            };
            this.MockPrnService
                .Setup(service => service.UpdatePrnStatus(id, statusUpdate))
                .Throws<Exception>();

            // Act
            var result = await this.PrnController.UpdatePrnStatus(id, statusUpdate);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCode.StatusCode);
        }

        [TestMethod]
        public async Task UpdatePrnData_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PackageRecyclingNoteRequest
            {
                //Note = "Test Status Update",
            };

            // Act
            var result = await this.PrnController.UpdatePrn(id, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            MockPrnService.Verify(s => s.UpdatePrn(id, request), Times.Once());
        }

        [TestMethod]
        public async Task UpdatePrnData_PrnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PackageRecyclingNoteRequest
            {
                //Note = "Test Status Update",
            };
            this.MockPrnService
                .Setup(service => service.UpdatePrn(id, request))
                .Throws<NotFoundException>();

            // Act
            var result = await this.PrnController.UpdatePrn(id, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task UpdatePrnData_MiscError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PackageRecyclingNoteRequest
            {
                //Note = "Test Status Update",
            };
            this.MockPrnService
                .Setup(service => service.UpdatePrn(id, request))
                .Throws<Exception>();

            // Act
            var result = await this.PrnController.UpdatePrn(id, request);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCode.StatusCode);
        }

        [TestMethod]
        public async Task DeletePrn_Success()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await this.PrnController.DeletePrn(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            MockPrnService.Verify(s => s.DeletePrn(id), Times.Once());
        }

        [TestMethod]
        public async Task DeletePrn_PrnNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.DeletePrn(id))
                .Throws<NotFoundException>();

            // Act
            var result = await this.PrnController.DeletePrn(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)statusCode.StatusCode);
        }

        [TestMethod]
        public async Task DeletePrn_MiscError()
        {
            // Arrange
            var prnId = Guid.NewGuid();
            this.MockPrnService
                .Setup(service => service.DeletePrn(prnId))
                .Throws<Exception>();

            // Act
            var result = await this.PrnController.DeletePrn(prnId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCode = (StatusCodeResult)result;
            Assert.AreEqual(HttpStatusCode.InternalServerError, (HttpStatusCode)statusCode.StatusCode);
        }


    }
}
