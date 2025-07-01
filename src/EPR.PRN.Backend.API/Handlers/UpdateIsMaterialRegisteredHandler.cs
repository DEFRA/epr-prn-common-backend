using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers;

public class UpdateIsMaterialRegisteredHandler(IRegistrationMaterialRepository repository)
	: IRequestHandler<UpdateIsMaterialRegisteredCommand>
{
	public async Task Handle(UpdateIsMaterialRegisteredCommand command, CancellationToken cancellationToken)
	{
		// Update IsMaterialRegistered
		await repository
			.UpdateIsMaterialRegisteredAsync(command.UpdateIsMaterialRegisteredDto);
	}
}
