using EPR.PRN.Backend.Data.DTO;
using MediatR;

namespace EPR.PRN.Backend.API.Commands
{
    public class UpsertInterimSiteCommand : IRequest
    {
        public SaveInterimSitesRequestDto? InterimSitesRequestDto { get; set; }
    }
}
