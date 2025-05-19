using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles;

public class RegistrationMaterialProfile : Profile
{
    public RegistrationMaterialProfile()
    {
        CreateMap<Registration, RegistrationOverviewDto>()
        //.ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationId + "_Green Ltd"))
        .ForMember(dest => dest.Regulator, opt => opt.MapFrom(_ => "EA"))
        .ForMember(dest => dest.OrganisationType,
            opt => opt.MapFrom(src => (ApplicationOrganisationType)src.ApplicationTypeId))
        .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src =>
             src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress)
                : string.Empty))
        .ForMember(dest => dest.Materials, opt => opt.MapFrom(src =>src.Materials.Where(m => m.IsMaterialRegistered))); 

        CreateMap<RegistrationMaterial, RegistrationMaterialDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))
            .ForMember(dest => dest.RegistrationReferenceNumber, opt => opt.MapFrom(src => src.RegistrationReferenceNumber));

        CreateMap<RegulatorRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));

        CreateMap<RegulatorApplicationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));

        CreateMap<RegistrationMaterial, RegistrationMaterialDetailsDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (RegistrationMaterialStatus?)src.StatusId));

        CreateMap<RegistrationMaterial, RegistrationMaterialReprocessingIODto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.SourcesOfPackagingWaste, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TypeOfSupplier))
            .ForMember(dest => dest.PlantEquipmentUsed, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().PlantEquipmentUsed))
            .ForMember(dest => dest.ReprocessingPackagingWasteLastYearFlag, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ReprocessingPackagingWasteLastYearFlag))
            .ForMember(dest => dest.UKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().UKPackagingWasteTonne))
            .ForMember(dest => dest.NonUKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().NonUKPackagingWasteTonne))
            .ForMember(dest => dest.NotPackingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().NotPackingWasteTonne))
            .ForMember(dest => dest.SenttoOtherSiteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().SenttoOtherSiteTonne))
            .ForMember(dest => dest.ContaminantsTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ContaminantsTonne))
            .ForMember(dest => dest.ProcessLossTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ProcessLossTonne))
            .ForMember(dest => dest.TotalInputs, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TotalInputs))
            .ForMember(dest => dest.TotalOutputs, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TotalOutputs));

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
            .ForMember(dest => dest.PermitType, opt => opt.MapFrom(src => src.PermitType!.Name))
            .ForMember(dest => dest.LicenceNumbers, opt => opt.MapFrom(src => GetReferenceNumber(src)))
            .ForMember(dest => dest.CapacityTonne, opt => opt.MapFrom(src => GetAuthorisedCapacityTonne(src)))
            .ForMember(dest => dest.CapacityPeriod, opt => opt.MapFrom(src => GetReferencePeriod(src)))
            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.MaximumReprocessingPeriod, opt => opt.MapFrom(src => src.MaximumReprocessingPeriod!.Name))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Registration, RegistrationSiteAddressDto>()
           .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.NationId : 0))
           .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
           .ForMember(dest => dest.GridReference, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.GridReference : string.Empty))
           .ForMember(dest => dest.LegalCorrespondenceAddress, opt => opt.MapFrom(src => src.LegalDocumentAddress != null ? CreateAddressString(src.LegalDocumentAddress) : string.Empty));

        CreateMap<Registration, MaterialsAuthorisedOnSiteDto>()
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Id))
           // .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationId + "_Green Ltd"))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.MaterialsAuthorisation, opt => opt.MapFrom(src => src.Materials));

        CreateMap<RegistrationMaterial, MaterialsAuthorisedOnSiteInfoDto>()
           .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
           .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.ReasonforNotreg))
           .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered));

        CreateMap<RegistrationMaterial, MaterialPaymentFeeDto>()
           .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.Registration.OrganisationId))
           .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src =>(ApplicationOrganisationType) src.Registration.ApplicationTypeId))
           .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
           .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.ApplicationReferenceNumber??string.Empty))           
           .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.Registration.ReprocessingSiteAddress) : string.Empty))
           .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? src.Registration.ReprocessingSiteAddress.NationId : 0))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName));
    }

    private static string[] GetReferenceNumber(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => src.MaterialExemptionReferences != null ? src.MaterialExemptionReferences.Select(x => x.ReferenceNo).ToArray() : [],
        PermitTypes.PollutionPreventionAndControlPermit => src.PPCPermitNumber != null ? [src.PPCPermitNumber] : [],
        PermitTypes.WasteManagementLicence => src.WasteManagementLicenceNumber != null ? [src.WasteManagementLicenceNumber] : [],
        PermitTypes.InstallationPermit => src.InstallationPermitNumber != null ? [src.InstallationPermitNumber] : [],
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => src.EnvironmentalPermitWasteManagementNumber != null ? [src.EnvironmentalPermitWasteManagementNumber] : [],
        _ => throw new RegulatorInvalidOperationException("Permit Type Not Valid")
    };

    private static decimal? GetAuthorisedCapacityTonne(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => null,
        PermitTypes.PollutionPreventionAndControlPermit => src.PPCReprocessingCapacityTonne,
        PermitTypes.WasteManagementLicence => src.WasteManagementReprocessingCapacityTonne,
        PermitTypes.InstallationPermit => src.InstallationReprocessingTonne,
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => src.EnvironmentalPermitWasteManagementTonne,
        _ => throw new RegulatorInvalidOperationException("Permit Type Not Valid")
    };

    private static string? GetReferencePeriod(RegistrationMaterial src) => src.PermitType?.Name switch
    {
        PermitTypes.WasteExemption => null,
        PermitTypes.PollutionPreventionAndControlPermit => src.PPCPeriod!.Name,
        PermitTypes.WasteManagementLicence => src.WasteManagementPeriod!.Name,
        PermitTypes.InstallationPermit => src.InstallationPeriod!.Name,
        PermitTypes.EnvironmentalPermitOrWasteManagementLicence => src.EnvironmentalPermitWasteManagementPeriod!.Name,
        _ => throw new RegulatorInvalidOperationException("Permit Type Not Valid")
    };


    private string CreateAddressString(Address reprocessingSiteAddress) =>
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