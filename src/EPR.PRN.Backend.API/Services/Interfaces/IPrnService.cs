namespace EPR.PRN.Backend.API.Services.Interfaces
{
    using DTO = EPR.PRN.Backend.API.Common.DTO;

    public interface IPrnService
    {
        Task<DTO.PrnDTo> GetPrnById(int Id);
    }
}
