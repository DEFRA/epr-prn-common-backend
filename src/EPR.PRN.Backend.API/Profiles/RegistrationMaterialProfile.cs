using AutoMapper;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        CreateMap<RegistrationMaterial, MaterialreprocessingIODto>();
        CreateMap<RegistrationMaterial, MaterialSamplingPlanDto>();
        CreateMap<RegistrationMaterial, MaterialWasteLicensesDto>()
            .ForMember(dest => dest.PermitType, opt => opt.MapFrom(src => src.PermitType.Name))

            .ForMember(dest => dest.WasteExemption, opt => opt.MapFrom(src => src.MaterialExemptionReferences.Select(x => x.ReferenceNo)))
            .ForMember(dest => dest.PPCPermitNumber, opt => opt.MapFrom(src => src.PPCPermitNumber))
            .ForMember(dest => dest.WasteManagementLicenseNumber, opt => opt.MapFrom(src => src.WasteManagementLicenseNumber))
            .ForMember(dest => dest.InstallationPermitNumber, opt => opt.MapFrom(src => src.InstallationPermitNumber))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementNumber, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementNumber))

            .ForMember(dest => dest.PPCReprocessingCapacityTonne, opt => opt.MapFrom(src => src.PPCReprocessingCapacityTonne))
            .ForMember(dest => dest.WasteManagementReprocessingCapacityTonne, opt => opt.MapFrom(src => src.WasteManagementReprocessingCapacityTonne))
            .ForMember(dest => dest.InstallationReprocessingTonne, opt => opt.MapFrom(src => src.InstallationReprocessingTonne))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementTonne, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementTonne))

            .ForMember(dest => dest.PPCPeriod, opt => opt.MapFrom(src => src.PPCPeriod.Name))
            .ForMember(dest => dest.WasteManagementPeriod, opt => opt.MapFrom(src => src.WasteManagementPeriod.Name))
            .ForMember(dest => dest.InstallationPeriod, opt => opt.MapFrom(src => src.InstallationPeriod.Name))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementPeriod, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementPeriod.Name))

            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.MaximumReprocessingPeriod, opt => opt.MapFrom(src => src.MaximumReprocessingPeriod.Name));
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