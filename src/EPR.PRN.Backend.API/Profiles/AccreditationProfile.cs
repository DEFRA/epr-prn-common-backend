using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using EPR.PRN.Backend.API.Dto.Accreditation;
using EPR.PRN.Backend.Data.DataModels.Accreditations;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles;

[ExcludeFromCodeCoverage]
public class AccreditationProfile : Profile
{
    public AccreditationProfile()
    {
        CreateMap<AccreditationEntity, AccreditationDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName));
        CreateMap<AccreditationRequestDto, AccreditationEntity>();

        CreateMap<Data.DataModels.Accreditations.AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, Data.DataModels.Accreditations.AccreditationPrnIssueAuth>();

        //--------------------------------------------------------------------------------------------------


        CreateMap<Accreditation, AccreditationDto>()



            // Example: Map nested MaterialName
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))
            // Example: Map AccreditationStatusName from nested AccreditationStatus.Name
            .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatusId))
            // Example: Map ApplicationTypeName from nested ApplicationType.Name
            .ForMember(dest => dest.ApplicationTypeId, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ApplicationTypeId))
            // Example: Map RegistrationMaterialId if property names differ
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
            // Example: Map CreatedDate if property names differ
            //  .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedOn))
            // Add more custom mappings as needed for other non-identical but related members
            .ForMember(des => des.AccreferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))
            
            .ForMember(des => des.AccreditationYear, opt => opt.MapFrom(src => src.AccreditationYear))

            // percentages
            .ForMember(des => des.NewMarketsPercentage, opt => opt.MapFrom(src => src.NewMarketsPercentage))
            .ForMember(des => des.BusinessCollectionsPercentage, opt => opt.MapFrom(src => src.BusinessCollectionsPercentage))
            .ForMember(des => des.InfrastructurePercentage, opt => opt.MapFrom(src => src.InfrastructurePercentage))
            .ForMember(des => des.CommunicationsPercentage, opt => opt.MapFrom(src => src.CommunicationsPercentage))            
            .ForMember(des => des.OtherPercentage, opt => opt.MapFrom(src => src.NotCoveredOtherCategoriesPercentage))
            .ForMember(des => des.NewUsesPercentage, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWastePercentage))
            .ForMember(des => des.PackagingWastePercentage, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWastePercentage))

            .ForMember(des => des.NewMarketsNotes, opt => opt.MapFrom(src => src.NewMarketsNotes))
            .ForMember(des => des.BusinessCollectionsNotes, opt => opt.MapFrom(src => src.BusinessCollectionsNotes))
            .ForMember(des => des.InfrastructureNotes, opt => opt.MapFrom(src => src.InfrastructureNotes))
            .ForMember(des => des.CommunicationsNotes, opt => opt.MapFrom(src => src.CommunicationsNotes))
            .ForMember(des => des.OtherNotes, opt => opt.MapFrom(src => src.NotCoveredOtherCategoriesNotes))
            .ForMember(des => des.NewUsesNotes, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWasteNotes))
            .ForMember(des => des.PackagingWasteNotes, opt => opt.MapFrom(src => src.NewUsersRecycledPackagingWasteNotes))
            ;
        CreateMap<AccreditationRequestDto, Accreditation>()
            // Example: Map ApplicationTypeId if property names differ
           // .ForMember(dest => dest.RegistrationMaterial.Registration.ApplicationTypeId, opt => opt.MapFrom(src => src.ApplicationTypeId))
            // Example: Map AccreditationStatusId if property names differ
            .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatusId))
            // Example: Map RegistrationMaterialId if property names differ
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
            // Example: Map ApplicationReferenceNumber if property names differ
            .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.AccreferenceNumber))
            // Example: Map CreatedOn if property names differ
            //.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedDate))
            // Add more custom mappings as needed for other non-identical but related members
            .ForMember(dest => dest.AccreditationYear, opt => opt.MapFrom(src => src.AccreditationYear))
            // percentages

            .ForMember(des => des.NewMarketsPercentage, opt => opt.MapFrom(src => src.NewMarketsPercentage))
            .ForMember(des => des.BusinessCollectionsPercentage, opt => opt.MapFrom(src => src.BusinessCollectionsPercentage))
            .ForMember(des => des.InfrastructurePercentage, opt => opt.MapFrom(src => src.InfrastructurePercentage))
            .ForMember(des => des.CommunicationsPercentage, opt => opt.MapFrom(src => src.CommunicationsPercentage))
            //.ForMember(des => des.NewUsersRecycledPackagingWastePercentage, opt => opt.MapFrom(src => src.NewUsesPercentage))
            .ForMember(des => des.NotCoveredOtherCategoriesPercentage, opt => opt.MapFrom(src => src.OtherPercentage))            
            .ForMember(des => des.NewUsersRecycledPackagingWastePercentage, opt => opt.MapFrom(src => src.NewUsesPercentage))


            .ForMember(des => des.NewMarketsNotes, opt => opt.MapFrom(src => src.NewMarketsNotes))
            .ForMember(des => des.BusinessCollectionsNotes, opt => opt.MapFrom(src => src.BusinessCollectionsNotes))
            .ForMember(des => des.InfrastructureNotes, opt => opt.MapFrom(src => src.InfrastructureNotes))
            .ForMember(des => des.CommunicationsNotes, opt => opt.MapFrom(src => src.CommunicationsNotes))
            
            .ForMember(des => des.NotCoveredOtherCategoriesNotes, opt => opt.MapFrom(src => src.OtherNotes))
            .ForMember(des => des.NewUsersRecycledPackagingWasteNotes, opt => opt.MapFrom(src => src.NewUsesNotes))
            ;


        CreateMap<Data.DataModels.Registrations.AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, Data.DataModels.Registrations.AccreditationPrnIssueAuth>();

        CreateMap<Data.DataModels.Accreditations.OverseasAccreditationSite, OverseasAccreditationSiteDto>();            
        CreateMap<OverseasAccreditationSiteDto, Data.DataModels.Accreditations.OverseasAccreditationSite>();

        CreateMap<Data.DataModels.Registrations.OverseasAccreditationSite, OverseasAccreditationSiteDto>();
        CreateMap<OverseasAccreditationSiteDto, Data.DataModels.Registrations.OverseasAccreditationSite>();
    }
}
