using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;

public class UpdateRegulatorRegistrationTaskHandler : IRequestHandler<UpdateRegulatorRegistrationTaskCommand, bool>
{
    private readonly IRegulatorRegistrationTaskStatusRepository _regulatorRegistrationTaskStatusRepository;
    public UpdateRegulatorRegistrationTaskHandler(IRegulatorRegistrationTaskStatusRepository regulatorRegistrationTaskStatusRepository)
    {
        _regulatorRegistrationTaskStatusRepository = regulatorRegistrationTaskStatusRepository;
    }
        
    public async Task<bool> Handle(UpdateRegulatorRegistrationTaskCommand command, CancellationToken cancellationToken)
    {
         await _regulatorRegistrationTaskStatusRepository.UpdateStatusAsync(command.Id, command.Status, command.Comment);

        return true;
    }

}