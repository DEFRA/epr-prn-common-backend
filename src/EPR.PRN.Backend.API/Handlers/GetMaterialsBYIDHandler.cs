using EPR.PRN.Backend.API.Models.ReadModel;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class GetMaterialsBYIDHandler : IRequestHandler<GetAllMaterialsByIdQuery, RegistrationMaterialTaskReadModel>
{
    private readonly IRepository _Repository ;
    public GetMaterialsBYIDHandler(IRepository Repository)
    {
        _Repository = Repository;
    }
        
    public async Task<RegistrationMaterialTaskReadModel> Handle(GetAllMaterialsByIdQuery request, CancellationToken cancellationToken)
    {
        RegistrationMaterialTaskReadModel result =  await _Repository.GetMaterialsBYID(request.RegistrationID);

        return result;


    }

}