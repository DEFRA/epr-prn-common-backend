using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
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

        CreateMap<RegistrationMaterial, RegistrationMaterialReprocessingIODto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.SourcesOfPackagingWaste, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().TypeOfSupplier))
            .ForMember(dest => dest.PlantEquipmentUsed, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().PlantEquipmentUsed))
            .ForMember(dest => dest.ReprocessingPackagingWasteLastYearFlag, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().ReprocessingPackagingWasteLastYearFlag))
            .ForMember(dest => dest.UKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().UKPackagingWasteTonne))
            .ForMember(dest => dest.NonUKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().NonUKPackagingWasteTonne))
            .ForMember(dest => dest.NotPackingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().NotPackingWasteTonne))
            .ForMember(dest => dest.SenttoOtherSiteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().SenttoOtherSiteTonne))
            .ForMember(dest => dest.ContaminantsTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().ContaminantsTonne))
            .ForMember(dest => dest.ProcessLossTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().ProcessLossTonne))
            .ForMember(dest => dest.TotalInput, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().TotalInput))
            .ForMember(dest => dest.TotalOutput, opt => opt.MapFrom(src => src.RegistrationReprocessingIO.Single().TotalOutput));

        CreateMap<RegistrationMaterial, RegistrationMaterialSamplingPlanDto>()                        
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName));

        CreateMap<RegistrationMaterial, RegistrationMaterialSamplingPlanDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.FileUploads));

        CreateMap<FileUpload, RegistrationMaterialSamplingPlanFileDto>()
            .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.Filename))
            .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.FileId))
            .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.FileUploadType, opt => opt.MapFrom(src => src.FileUploadType.Name))
            .ForMember(dest => dest.FileUploadStatus, opt => opt.MapFrom(src => src.FileUploadStatus.Name))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

        CreateMap<RegistrationMaterial, RegistrationMaterialWasteLicencesDto>()
            .ForMember(dest => dest.PermitType, opt => opt.MapFrom(src => src.PermitType.Name))
            .ForMember(dest => dest.LicenceNumbers, opt => opt.MapFrom(src => GetReferenceNumber(src)))
            .ForMember(dest => dest.CapacityTonne, opt => opt.MapFrom(src => GetAuthorisedCapacityTonne(src)))
            .ForMember(dest => dest.CapacityPeriod, opt => opt.MapFrom(src => GetReferencePeriod(src)))
            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.MaximumReprocessingPeriod, opt => opt.MapFrom(src => src.MaximumReprocessingPeriod.Name))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName));
    }


    private static string?[]? GetReferenceNumber(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => src.MaterialExemptionReferences?.Select(x => x.ReferenceNo).ToArray(),
        PermitTypes.PollutionPreventionAndControlPermit => [src.PPCPermitNumber],
        PermitTypes.WasteManagementLicence => [src.WasteManagementLicenceNumber],
        PermitTypes.InstallationPermit => [src.InstallationPermitNumber],
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => [src.EnvironmentalPermitWasteManagementNumber],
        _ => null
    };

    private static decimal? GetAuthorisedCapacityTonne(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => null,
        PermitTypes.PollutionPreventionAndControlPermit => src.PPCReprocessingCapacityTonne,
        PermitTypes.WasteManagementLicence => src.WasteManagementReprocessingCapacityTonne,
        PermitTypes.InstallationPermit => src.InstallationReprocessingTonne,
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => src.EnvironmentalPermitWasteManagementTonne,
        _ => null
    };

    private static string? GetReferencePeriod(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => null,
        PermitTypes.PollutionPreventionAndControlPermit => src.PPCPeriod?.Name,
        PermitTypes.WasteManagementLicence => src.WasteManagementPeriod?.Name,
        PermitTypes.InstallationPermit => src.InstallationPeriod?.Name,
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => src.EnvironmentalPermitWasteManagementPeriod?.Name,
        _ => null
    };


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