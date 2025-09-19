using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using FluentAssertions;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class RegulatorAccreditationMarkAsDulyMadeHandlerTests
{
    private Mock<IRegulatorAccreditationRepository> _rmRepositoryMock;
    private RegulatorAccreditationMarkAsDulyMadeHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _rmRepositoryMock = new Mock<IRegulatorAccreditationRepository>();
        _handler = new RegulatorAccreditationMarkAsDulyMadeHandler(_rmRepositoryMock.Object);
    }

    [TestMethod]
    public async Task Handle_MaterialExists_ExecutesSuccessfully()
    {
        // Arrange
        var command = new RegulatorAccreditationMarkAsDulyMadeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            DeterminationDate = DateTime.UtcNow,
            DulyMadeDate = DateTime.UtcNow.AddDays(-1),
            DulyMadeBy = new Guid("CE564609-4455-4C29-818B-497F06567A6C")
        };

        var accreditation = new Accreditation { ExternalId = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"), ApplicationReferenceNumber = "A1234" };
        _rmRepositoryMock.Setup(x => x.GetAccreditationById(Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"))).ReturnsAsync(accreditation);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        _rmRepositoryMock.Verify(x => x.AccreditationMarkAsDulyMade(
            Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            (int)RegulatorTaskStatus.Completed,
             command.DulyMadeDate,
            command.DeterminationDate,          
            command.DulyMadeBy
        ), Times.Once);
    }

    [TestMethod]
    public async Task Handle_MaterialDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = new RegulatorAccreditationMarkAsDulyMadeCommand
        {
            Id = Guid.Parse("a9421fc1-a912-42ee-85a5-3e06408759a9"),
            DeterminationDate = DateTime.UtcNow,
            DulyMadeDate = DateTime.UtcNow.AddDays(-1),
            DulyMadeBy =new Guid("CE564609-4455-4C29-818B-497F06567A6C")
        };

        _rmRepositoryMock.Setup(x => x.GetAccreditationById(command.Id))
                         .ReturnsAsync((Accreditation)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Accreditation not found.");
    }
}
