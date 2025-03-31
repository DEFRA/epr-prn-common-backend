using EPR.PRN.Backend.API.Repositories.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class UpdateRegulatorApplicationTaskHandler : IRequestHandler<UpdateRegulatorApplicationTaskCommand, bool>
{
    private readonly IRepository _Repository ;
    public UpdateRegulatorApplicationTaskHandler(IRepository Repository)
    {
        _Repository = Repository;
    }
        
    public async Task<bool> Handle(UpdateRegulatorApplicationTaskCommand command, CancellationToken cancellationToken)
    {
        //RegistrationMaterialTaskReadModel result =  await _Repository.GetMaterialsBYID(command.Id);

        return true;


    }

}