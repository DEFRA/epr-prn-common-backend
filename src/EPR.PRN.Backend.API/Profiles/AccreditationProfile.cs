﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.Data.DataModels.Accreditations;

namespace EPR.PRN.Backend.API.Profiles;

[ExcludeFromCodeCoverage]
public class AccreditationProfile : Profile
{
    public AccreditationProfile()
    {
        CreateMap<AccreditationEntity, AccreditationDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName));
        CreateMap<AccreditationRequestDto, AccreditationEntity>();

        CreateMap<AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, AccreditationPrnIssueAuth>();
    }
}