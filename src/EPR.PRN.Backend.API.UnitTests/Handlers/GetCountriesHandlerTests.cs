using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers
{
    [TestClass]
    public class GetCountriesHandlerTests
    {
        [TestMethod]
        public async Task Handle_ReturnsCountryNames()
        {
            // Arrange
            var countries = new List<LookupCountry>
            {
                new LookupCountry { Id = 1, Name = "UK" },
                new LookupCountry { Id = 2, Name = "France" }
            };
            var repoMock = new Mock<ILookupCountryRepository>();
            repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(countries);

            var handler = new GetCountriesHandler(repoMock.Object);

            // Act
            var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(countries.Select(c => c.Name));
        }
    }
}
