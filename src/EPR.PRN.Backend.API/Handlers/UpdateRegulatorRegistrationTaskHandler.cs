using EPR.PRN.Backend.API.Models.ReadModel;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class UpdateRegulatorRegistrationTaskHandler : IRequestHandler<UpdateRegulatorRegistrationTaskCommand, bool>
{
    private readonly IRepository _Repository ;
    public UpdateRegulatorRegistrationTaskHandler(IRepository Repository)
    {
        _Repository = Repository;
    }
        
    public async Task<bool> Handle(UpdateRegulatorRegistrationTaskCommand command, CancellationToken cancellationToken)
    {
        //RegistrationMaterialTaskReadModel result =  await _Repository.GetMaterialsBYID(command.Id);

        return true;


    }

}