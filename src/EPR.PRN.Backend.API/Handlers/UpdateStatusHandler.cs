using EPR.PRN.Backend.API.Commands;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Repositories;
using EPR.PRN.Backend.API.Repositories.Interfaces;
using EPR.PRN.Backend.Data.DataModels;
using EPR.PRN.Backend.Data.Interfaces;
using EPR.PRN.Backend.Data.Repositories;
using MediatR;
namespace EPR.PRN.Backend.API.Handlers;
public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand, int>
{
    private readonly IPrnRepository _prnRepository ;
    public UpdateStatusHandler(IPrnRepository prnRepository)
    {
        _prnRepository = prnRepository;
    }
        
    public async Task<int> Handle(UpdateStatusCommand command, CancellationToken cancellationToken)
    {
       PrnStatusHistory prnStatusHistory = new PrnStatusHistory();
        //var prnStatusHistory = new PrnStatusHistory()
        //{
        //    PrnIdFk = prn.Id,
        //    PrnStatusIdFk = (int)prnUpdate.Status,
        //    CreatedOn = updateDate,
        //    CreatedByUser = userId,
        //};

        
        _prnRepository.AddPrnStatusHistory(prnStatusHistory);

        return command.StatusID;
    }

}