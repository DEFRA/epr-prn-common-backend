using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.PRN.Backend.Data.UnitTests.Repositories
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SaveAndContinueRepositoryTests
    {
        private SaveAndContinueRepository _repository;
        private Mock<EprContext> _mockEprContext;


        [TestInitialize]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<EprContext>().Options;
            _mockEprContext = new Mock<EprContext>(dbContextOptions);
            _mockEprContext.Setup(context => context.SaveAndContinue).ReturnsDbSet(new List<SaveAndContinue>());
            _repository = new SaveAndContinueRepository(_mockEprContext.Object);
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddModel()
        {
            var model = new SaveAndContinue();
            // Act
            await _repository.AddAsync(model);

            // Assert
            _mockEprContext.Verify(x=>x.SaveAndContinue.AddAsync(It.IsAny<SaveAndContinue>(), It.IsAny<CancellationToken>()), Times.Once());
            _mockEprContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
