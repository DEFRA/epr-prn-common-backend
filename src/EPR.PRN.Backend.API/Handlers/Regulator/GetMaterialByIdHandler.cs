﻿using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Queries;
using EPR.PRN.Backend.Data.Interfaces.Regulator;
using MediatR;

namespace EPR.PRN.Backend.API.Handlers.Regulator;

public class GetMaterialByIdHandler(
    IRegistrationMaterialRepository rmRepository,
    IMapper mapper
) : IRequestHandler<GetMaterialDetailByIdQuery, RegistrationMaterialDetailsDto>
{
    public async Task<RegistrationMaterialDetailsDto> Handle(GetMaterialDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var materialEntity = await rmRepository.GetRegistrationMaterialById(request.Id);
        var materialDto = mapper.Map<RegistrationMaterialDetailsDto>(materialEntity);
        return materialDto;
    }
}