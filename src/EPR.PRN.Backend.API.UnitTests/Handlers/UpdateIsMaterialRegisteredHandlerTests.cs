
using AutoFixture;
using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using Moq;

namespace EPR.PRN.Backend.API.UnitTests.Handlers;

[TestClass]
public class UpdateIsMaterialRegisteredHandlerTests
{
	private Mock<IRegistrationMaterialRepository> _repositoryMock;
	private UpdateIsMaterialRegisteredHandler _handler;
	private static readonly Fixture _fixture = new();

	[TestInitialize]
	public void TestInitialize()
	{
		_repositoryMock = new Mock<IRegistrationMaterialRepository>();
		_handler = new UpdateIsMaterialRegisteredHandler(_repositoryMock.Object);
	}

	[TestMethod]
	public async Task Handle_ShouldCall_RepositoryMethods_WithCorrectParameters()
	{
		// Arrange
		var command = _fixture.Build<UpdateIsMaterialRegisteredCommand>()
			.Create();

		// Act
		await _handler.Handle(command, CancellationToken.None);

		// Assert
		_repositoryMock.Verify(r => r.UpdateIsMaterialRegisteredAsync(command.UpdateIsMaterialRegisteredDto), Times.Once);
	}
}
