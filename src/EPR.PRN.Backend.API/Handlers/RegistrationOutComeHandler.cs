using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data.Interfaces;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class RegistrationOutComeHandler : IRequestHandler<RegistrationOutComeCommand, bool>
{
    private readonly IRepository _Repository ;
    public RegistrationOutComeHandler(IRepository prnRepository)
    {
        _Repository = prnRepository;
    }
        
    public async Task<bool> Handle(RegistrationOutComeCommand request, CancellationToken cancellationToken)
    {

        // Update registration outcome

        int Outcome = (int)request.OutCome;
        string OutcomeComment = request.OutComeComment ?? string.Empty;

       bool result=  await _Repository.UpdateRegistrationOutCome(request.RegistrationID, request.MaterialID, Outcome, OutcomeComment);

        return result;


    }

}