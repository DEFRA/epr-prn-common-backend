using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.Data.DataModels;
using MediatR;
namespace EPR.PRN.Backend.API.Commands;
public class UpdateStatusCommand:IRequest<int>
{
    public Guid PrnId { get; set; }
    public int StatusID { get; set; }
    public UpdateStatusCommand(Guid PrnID, int statusid)
    {
        PrnId = PrnID;
        StatusID = (int)(EprnStatus)statusid;
          
    }
}