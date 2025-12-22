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


        CreateMap<AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, AccreditationPrnIssueAuth>();

        CreateMap<Accreditation, AccreditationDto>()       
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))            
            .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatusId))            
            .ForMember(dest => dest.ApplicationTypeId, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ApplicationTypeId))            
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
            .ForMember(des => des.AccreferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))            
            .ForMember(des => des.AccreditationYear, opt => opt.MapFrom(src => src.AccreditationYear))

            .ForMember(des => des.NewMarketsPercentage, opt => opt.MapFrom(src => src.NewMarketsPercentage))
            .ForMember(des => des.BusinessCollectionsPercentage, opt => opt.MapFrom(src => src.BusinessCollectionsPercentage))
            .ForMember(des => des.InfrastructurePercentage, opt => opt.MapFrom(src => src.InfrastructurePercentage))
            .ForMember(des => des.CommunicationsPercentage, opt => opt.MapFrom(src => src.CommunicationsPercentage))            
            .ForMember(des => des.OtherPercentage, opt => opt.MapFrom(src => src.NotCoveredOtherCategoriesPercentage))
            .ForMember(des => des.NewUsesPercentage, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWastePercentage))
            .ForMember(des => des.PackagingWastePercentage, opt => opt.MapFrom(src => src.RecycledWastePercentage))

            .ForMember(des => des.NewMarketsNotes, opt => opt.MapFrom(src => src.NewMarketsNotes))
            .ForMember(des => des.BusinessCollectionsNotes, opt => opt.MapFrom(src => src.BusinessCollectionsNotes))
            .ForMember(des => des.InfrastructureNotes, opt => opt.MapFrom(src => src.InfrastructureNotes))
            .ForMember(des => des.CommunicationsNotes, opt => opt.MapFrom(src => src.CommunicationsNotes))
            .ForMember(des => des.OtherNotes, opt => opt.MapFrom(src => src.NotCoveredOtherCategoriesNotes))
            .ForMember(des => des.NewUsesNotes, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWasteNotes))
            .ForMember(des => des.PackagingWasteNotes, opt => opt.MapFrom(src => src.RecycledWasteNotes))
            ;
        CreateMap<AccreditationRequestDto, Accreditation>()
            .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatusId))
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
            .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.AccreferenceNumber))
            .ForMember(dest => dest.AccreditationYear, opt => opt.MapFrom(src => src.AccreditationYear))

            .ForMember(des => des.NewMarketsPercentage, opt => opt.MapFrom(src => src.NewMarketsPercentage))
            .ForMember(des => des.BusinessCollectionsPercentage, opt => opt.MapFrom(src => src.BusinessCollectionsPercentage))
            .ForMember(des => des.InfrastructurePercentage, opt => opt.MapFrom(src => src.InfrastructurePercentage))
            .ForMember(des => des.CommunicationsPercentage, opt => opt.MapFrom(src => src.CommunicationsPercentage))
            .ForMember(des => des.NotCoveredOtherCategoriesPercentage, opt => opt.MapFrom(src => src.OtherPercentage))
            .ForMember(des => des.NewUsersRecycledPackagingWastePercentage, opt => opt.MapFrom(src => src.NewUsesPercentage))
            .ForMember(des => des.RecycledWastePercentage, opt => opt.MapFrom(src => src.PackagingWastePercentage))



            .ForMember(des => des.NewMarketsNotes, opt => opt.MapFrom(src => src.NewMarketsNotes))
            .ForMember(des => des.BusinessCollectionsNotes, opt => opt.MapFrom(src => src.BusinessCollectionsNotes))
            .ForMember(des => des.InfrastructureNotes, opt => opt.MapFrom(src => src.InfrastructureNotes))
            .ForMember(des => des.CommunicationsNotes, opt => opt.MapFrom(src => src.CommunicationsNotes))
            .ForMember(des => des.NotCoveredOtherCategoriesNotes, opt => opt.MapFrom(src => src.OtherNotes))
            .ForMember(des => des.NewUsersRecycledPackagingWasteNotes, opt => opt.MapFrom(src => src.NewUsesNotes))
            .ForMember(des => des.RecycledWasteNotes, opt => opt.MapFrom(src => src.PackagingWasteNotes))
            ;

        CreateMap<AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, AccreditationPrnIssueAuth>();

        CreateMap<OverseasAccreditationSite, OverseasAccreditationSiteDto>();
        CreateMap<OverseasAccreditationSiteDto, OverseasAccreditationSite>();

        CreateMap<Accreditation, AccreditationOverviewDto>();
        CreateMap<RegistrationMaterial, RegistrationMaterialDto>();
    }
}
