using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Controllers;
using EPR.PRN.Backend.API.Queries;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Controllers.Lookups
{
    [TestClass]
    public class CountriesControllerTests
    {
        [TestMethod]
        public async Task GetCountries_ReturnsOkWithCountries()
        {
            // Arrange
            var expectedCountries = new List<string> { "UK", "France" };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCountriesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedCountries);

            var controller = new CountriesController(mediatorMock.Object);

            // Act
            var result = await controller.GetCountries();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(expectedCountries);
        }
    }
}
