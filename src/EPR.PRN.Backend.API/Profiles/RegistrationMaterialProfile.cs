using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles;

public class RegistrationMaterialProfile : Profile
{
    public RegistrationMaterialProfile()
    {
        CreateMap<Registration, RegistrationOverviewDto>()
            .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationId + "_Green Ltd"))
            .ForMember(dest => dest.Regulator, opt => opt.MapFrom(_ => "EA"))
            .ForMember(dest => dest.OrganisationType,
                opt => opt.MapFrom(src => (ApplicationOrganisationType)src.ApplicationTypeId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => CreateAddressString(src.ReprocessingSiteAddress)));

        CreateMap<RegistrationMaterial, RegistrationMaterialDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.RegistrationReferenceNumber, opt => opt.MapFrom(src => src.ReferenceNumber));

        CreateMap<RegulatorRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));
           
        CreateMap<RegulatorApplicationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));

        CreateMap<RegistrationMaterial, RegistrationMaterialDetailsDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (RegistrationMaterialStatus?)src.StatusID));
        CreateMap<Registration, RegistrationSiteAddressDto>()
            .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.ReprocessingSiteAddress.NationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => CreateAddressString(src.ReprocessingSiteAddress)))
            .ForMember(dest => dest.GridReference, opt => opt.MapFrom(src => src.ReprocessingSiteAddress.GridReference ?? ""))
             .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => CreateAddressString(src.LegalDocumentAddress)));
         CreateMap<Registration, RegistrationSiteAddressDto>()
            .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.NationId : 0))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.GridReference, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.GridReference : string.Empty))
            .ForMember(dest => dest.LegalCorrespondenceAddress, opt => opt.MapFrom(src => src.LegalDocumentAddress != null ? CreateAddressString(src.LegalDocumentAddress) : string.Empty));

        CreateMap<Registration, MaterialsAuthorisedOnSiteDto>()
            .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationId + "_Green Ltd"))
           .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
           .ForMember(dest => dest.MaterialsAuthorisation, opt => opt.MapFrom(src => src.Materials));

        CreateMap<RegistrationMaterial, MaterialsAuthorisedOnSiteInfoDto>()
           .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material.MaterialName))
           .ForMember(dest => dest.RegistrationStatus, opt => opt.MapFrom(_ => RegulatorTaskStatus.NotStarted.ToString()))
           .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ReasonforNotreg))
           .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered));
    }

    private string CreateAddressString(LookupAddress reprocessingSiteAddress) =>
        string.Join(
            ", ",
            new[]
            {
                reprocessingSiteAddress.AddressLine1,
                reprocessingSiteAddress.AddressLine2,
                reprocessingSiteAddress.TownCity,
                reprocessingSiteAddress.County,
                reprocessingSiteAddress.PostCode
            }.Where(addressPart => !string.IsNullOrEmpty(addressPart)));
}