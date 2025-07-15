using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO.Accreditiation;

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

        CreateMap<AccreditationEntity, AccreditationOverviewDto>()
            .ForMember(dest => dest.ApplicationType, opts => opts.MapFrom(src => src.ApplicationType == null ? null : src.ApplicationType))
            .ForMember(dest => dest.AccreditationStatus, opts => opts.MapFrom(src => src.AccreditationStatus == null ? null : src.AccreditationStatus))
            .ForMember(dest => dest.RegistrationMaterial, opts => opts.MapFrom(src => src.RegistrationMaterial == null ? null : src.RegistrationMaterial));

        CreateMap<ApplicationType, ApplicationTypeDto>();
        CreateMap<AccreditationStatus, AccreditationStatusDto>();
        CreateMap<RegistrationMaterial, RegistrationMaterialDto>();

        CreateMap<OverseasAccreditationSite, OverseasAccreditationSiteDto>();
        CreateMap<OverseasAccreditationSiteDto, OverseasAccreditationSite>();
    }
}