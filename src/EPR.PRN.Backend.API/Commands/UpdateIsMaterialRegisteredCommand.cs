using EPR.PRN.Backend.Data.DTO;
using MediatR;

namespace EPR.PRN.Backend.API.Commands;

public class UpdateIsMaterialRegisteredCommand : IRequest
{
    public List<UpdateIsMaterialRegisteredDto> UpdateIsMaterialRegisteredDto { get; set; } = [];
}
