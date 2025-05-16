using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class RegistrationMaterialsMarkAsDulyMadeHandlerTests
{
    private Mock<IRegistrationMaterialRepository> _rmRepositoryMock;
    private RegistrationMaterialsMarkAsDulyMadeHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegistrationMaterialRepository>();
        _handler = new RegistrationMaterialsMarkAsDulyMadeHandler(_rmRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_MaterialExists_ExecutesSuccessfully()
    {
        // Arrange
        var command = new RegistrationMaterialsMarkAsDulyMadeCommand
        {
            RegistrationMaterialId = 1,
            DeterminationDate = DateTime.UtcNow,
            DulyMadeDate = DateTime.UtcNow.AddDays(-1),
            DulyMadeBy = new Guid("CE564609-4455-4C29-818B-497F06567A6C")
        };

        var material = new RegistrationMaterial { Id = 1 };
        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(1)).ReturnsAsync(material);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        _rmRepositoryMock.Verify(x => x.RegistrationMaterialsMarkAsDulyMade(
            1,
            (int)RegulatorTaskStatus.Completed,
            command.DeterminationDate,
            command.DulyMadeDate,
            command.DulyMadeBy
        ), Times.Once);
    }

    [TestMethod]
    public async Task Handle_MaterialDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new RegistrationMaterialsMarkAsDulyMadeCommand
        {
            RegistrationMaterialId = 99,
            DeterminationDate = DateTime.UtcNow,
            DulyMadeDate = DateTime.UtcNow.AddDays(-1),
            DulyMadeBy =new Guid("CE564609-4455-4C29-818B-497F06567A6C")
        };

        _rmRepositoryMock.Setup(x => x.GetRegistrationMaterialById(command.RegistrationMaterialId))
                         .ReturnsAsync((RegistrationMaterial)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Material not found.");
    }
}
