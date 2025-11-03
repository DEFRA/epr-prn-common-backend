using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class DeleteRegistrationMaterialHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _mockRegistrationMaterialRepository;
    private DeleteRegistrationMaterialHandler _handler;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockRegistrationMaterialRepository = new Mock<IRegistrationMaterialRepository>();
        _handler = new DeleteRegistrationMaterialHandler(_mockRegistrationMaterialRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldReturnMappedDto_WhenMaterialExists()
    {
        // Arrange
        var registrationMaterialId = Guid.NewGuid();
        var command = new DeleteRegistrationMaterialCommand
        {
            RegistrationMaterialId = registrationMaterialId
        };

        _mockRegistrationMaterialRepository
            .Setup(o => o.DeleteAsync(registrationMaterialId))
            .Returns(Task.CompletedTask);

        // Act & Assert
        var result = _handler.Handle(command, CancellationToken.None);
        await result;
        Assert.IsTrue(result.IsCompletedSuccessfully);
    }
}