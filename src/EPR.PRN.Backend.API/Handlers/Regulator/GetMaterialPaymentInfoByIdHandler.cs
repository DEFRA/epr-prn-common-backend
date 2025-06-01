using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialPaymentInfoByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialPaymentFeeByIdQuery, MaterialPaymentFeeDto>
{
    public async Task<MaterialPaymentFeeDto> Handle(GetMaterialPaymentFeeByIdQuery request, CancellationToken cancellationToken)
    {
        var registrationMaterial = await rmRepository.GetRegistrationMaterialById(request.Id);
        if (registrationMaterial?.Tasks != null)
        {
            var samplingTask = registrationMaterial.Tasks
                  .Where(t =>
                      t.Task?.Name == RegulatorTaskNames.CheckRegistrationStatus &&
                      t.Task?.ApplicationTypeId == registrationMaterial.Registration.ApplicationTypeId && t.Task.IsMaterialSpecific == true);
            registrationMaterial.Tasks = samplingTask.ToList()??new();
        }
        
        var materialPaymentFeeDto = registrationMaterial != null ? mapper.Map<MaterialPaymentFeeDto>(registrationMaterial) : new MaterialPaymentFeeDto();

        return materialPaymentFeeDto;
    }
}