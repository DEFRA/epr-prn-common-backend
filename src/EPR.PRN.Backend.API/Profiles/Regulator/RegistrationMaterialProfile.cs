﻿using System.Linq.Expressions;
using AutoMapper;
using EPR.PRN.Backend.API.Common.Constants;
using EPR.PRN.Backend.API.Common.Enums;
using EPR.PRN.Backend.API.Common.Exceptions;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.API.Handlers;
using EPR.PRN.Backend.Data.DataModels.Registrations;
using EPR.PRN.Backend.Data.DTO;

namespace EPR.PRN.Backend.API.Profiles.Regulator;

public class RegistrationMaterialProfile : Profile
{
    public RegistrationMaterialProfile()
    {
        CreateRegistrationMaterialMappings();
        CreateRegistrationMappings();
        CreateRegistrationMaterialDtoMappings();
        CreateAccreditationMappings();
        CreateTaskStatusMappings();
        CreateDetailsAndReprocessingMappings();
        CreateSamplingPlanMappings();
        CreateBusinessPlanAndPaymentMappings();
        CreateWasteLicencesMappings();
        CreateSiteMappings();
        CreateCarrierMappings();
        CreateMaterialsAuthorisedMappings();
        CreateMaterialPaymentFeeMappings();
        CreateDtoToEntityMappings();
        CreateLookupMappings();
        CreateRegistrationMaterialContactMappings();
        CreateRegistrationReprocessingIOMappings();
        CreateOverseasReprocessingMappings();
    }

    private void CreateRegistrationReprocessingIOMappings()
    {
        CreateMap<RegistrationReprocessingIORequestDto, RegistrationReprocessingIOCommand>();
        CreateMap<RegistrationReprocessingIORawMaterialOrProductsDto, RegistrationReprocessingIORawMaterialOrProducts>().ReverseMap();
        CreateMap<RegistrationReprocessingIOCommand, RegistrationReprocessingIO>()
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.Ignore());
    }

    private void CreateRegistrationMaterialMappings()
    {
        CreateMap<RegistrationMaterial, CreateRegistrationMaterialDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }

    private void CreateRegistrationMappings()
    {
        CreateMap<Registration, CreateRegistrationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));

        CreateMap<Registration, RegistrationOverviewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.Regulator, opt => opt.MapFrom(_ => "EA"))
            .ForMember(dest => dest.OrganisationType, opt => opt.MapFrom(src => (ApplicationOrganisationType)src.ApplicationTypeId))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom((src, dest, _, context) => MapTasks(src.Tasks, src.AccreditationTasks, context)))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.SiteGridReference, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.GridReference : string.Empty))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials.Where(m => m.IsMaterialRegistered)));
    }

    private void CreateRegistrationMaterialDtoMappings()
    {
        CreateMap<RegistrationMaterial, RegistrationMaterialDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))
            .ForMember(dest => dest.RegistrationReferenceNumber, opt => opt.MapFrom(src => src.RegistrationReferenceNumber));
    }

    private void CreateAccreditationMappings()
    {
        CreateMap<Accreditation, AccreditationDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.DeterminationDate, opt => opt.MapFrom(src => src.AccreditationDulyMade.FirstOrDefault().DeterminationDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.AccreditationStatus.Name))
            .ForMember(dest => dest.ApplicationReference, opt => opt.MapFrom(src => src.ApplicationReferenceNumber));
    }

    private void CreateTaskStatusMappings()
    {
        CreateMap<RegulatorAccreditationRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.AccreditationYear));

        CreateMap<RegulatorAccreditationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Accreditation.AccreditationYear));

        CreateMap<RegulatorRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));

        CreateMap<RegulatorApplicationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));

        CreateMap<ApplicantRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));
    }

    private void CreateDetailsAndReprocessingMappings()
    {
        CreateMap<RegistrationMaterial, RegistrationMaterialDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (RegistrationMaterialStatus?)src.StatusId))
            .ForMember(dest => dest.DulyMade, opt => opt.MapFrom(src => src.DulyMade!.DulyMadeDate))
            .ForMember(dest => dest.DeterminationDate, opt => opt.MapFrom(src => src.DeterminationDate.DeterminateDate));

        CreateMap<RegistrationMaterial, RegistrationMaterialReprocessingIODto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.SourcesOfPackagingWaste, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TypeOfSuppliers))
            .ForMember(dest => dest.PlantEquipmentUsed, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().PlantEquipmentUsed))
            .ForMember(dest => dest.ReprocessingPackagingWasteLastYearFlag, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ReprocessingPackagingWasteLastYearFlag))
            .ForMember(dest => dest.UKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().UKPackagingWasteTonne))
            .ForMember(dest => dest.NonUKPackagingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().NonUKPackagingWasteTonne))
            .ForMember(dest => dest.NotPackingWasteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().NotPackingWasteTonne))
            .ForMember(dest => dest.SenttoOtherSiteTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().SenttoOtherSiteTonne))
            .ForMember(dest => dest.ContaminantsTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ContaminantsTonne))
            .ForMember(dest => dest.ProcessLossTonne, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().ProcessLossTonne))
            .ForMember(dest => dest.TotalInputs, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TotalInputs))
            .ForMember(dest => dest.TotalOutputs, opt => opt.MapFrom(src => src.RegistrationReprocessingIO!.Single().TotalOutputs))
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.Registration.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.RegulatorApplicationTaskStatusId, opt => opt.MapFrom(src => GetApplicationTaskExternalId(src.Tasks, RegulatorTaskNames.ReprocessingInputsAndOutputs)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetApplicationTaskStatus(src.Tasks, RegulatorTaskNames.ReprocessingInputsAndOutputs)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetApplicationTaskNotes(src.Tasks, RegulatorTaskNames.ReprocessingInputsAndOutputs)));
    }

    private void CreateSamplingPlanMappings()
    {
        CreateMap<RegistrationMaterial, RegistrationMaterialSamplingPlanDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.FileUploads))
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.Registration.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.RegulatorApplicationTaskStatusId, opt => opt.MapFrom(src => GetApplicationTaskExternalId(src.Tasks, RegulatorTaskNames.SamplingAndInspectionPlan)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetApplicationTaskStatus(src.Tasks, RegulatorTaskNames.SamplingAndInspectionPlan)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetApplicationTaskNotes(src.Tasks, RegulatorTaskNames.SamplingAndInspectionPlan)));

        CreateMap<RegistrationFileUpload, RegistrationMaterialSamplingPlanFileDto>()
            .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.Filename))
            .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.FileId))
            .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.FileUploadType, opt => opt.MapFrom(src => src.FileUploadType!.Name))
            .ForMember(dest => dest.FileUploadStatus, opt => opt.MapFrom(src => src.FileUploadStatus!.Name))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

        CreateMap<RegistrationMaterial, AccreditationSamplingPlanDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName));

        CreateMap<AccreditationFileUpload, AccreditationSamplingPlanFileDto>()
            .ForMember(dest => dest.Filename, opt => opt.MapFrom(src => src.Filename))
            .ForMember(dest => dest.FileId, opt => opt.MapFrom(src => src.FileId))
            .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.FileUploadType, opt => opt.MapFrom(src => src.FileUploadType!.Name))
            .ForMember(dest => dest.FileUploadStatus, opt => opt.MapFrom(src => src.FileUploadStatus!.Name));
    }

    private void CreateBusinessPlanAndPaymentMappings()
    {
        CreateMap<Accreditation, AccreditationBusinessPlanDto>()
            .ForMember(dest => dest.AccreditationId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.RegistrationMaterial.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetAccreditationTaskStatus(src.Tasks, RegulatorTaskNames.BusinessPlan)));

        CreateMap<Accreditation, AccreditationSamplingPlanDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))
            .ForMember(dest => dest.Files, opt => opt.MapFrom(src => src.FileUploads));

        CreateMap<Accreditation, AccreditationPaymentFeeDetailsDto>()
            .ForMember(dest => dest.AccreditationId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.RegistrationMaterial.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))
            .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(src => src.CreatedOn))
            .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ApplicationTypeId))
            .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.RegistrationMaterial.Registration.ReprocessingSiteAddress.NationId != null ? src.RegistrationMaterial.Registration.ReprocessingSiteAddress.NationId : 0));
    }

    private void CreateWasteLicencesMappings()
    {
        CreateMap<RegistrationMaterial, RegistrationMaterialWasteLicencesDto>()
            .ForMember(dest => dest.PermitType, opt => opt.MapFrom(src => src.PermitType!.Name))
            .ForMember(dest => dest.LicenceNumbers, opt => opt.MapFrom(src => GetReferenceNumber(src)))
            .ForMember(dest => dest.CapacityTonne, opt => opt.MapFrom(src => GetAuthorisedCapacityTonne(src)))
            .ForMember(dest => dest.CapacityPeriod, opt => opt.MapFrom(src => GetReferencePeriod(src)))
            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.MaximumReprocessingPeriod, opt => opt.MapFrom(src => src.MaximumReprocessingPeriod!.Name))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.Registration.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.RegulatorApplicationTaskStatusId, opt => opt.MapFrom(src => GetApplicationTaskExternalId(src.Tasks, RegulatorTaskNames.WasteLicensesPermitsAndExemptions)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetApplicationTaskStatus(src.Tasks, RegulatorTaskNames.WasteLicensesPermitsAndExemptions)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetApplicationTaskNotes(src.Tasks, RegulatorTaskNames.WasteLicensesPermitsAndExemptions)));
    }

    private void CreateSiteMappings()
    {
        CreateMap<Registration, RegistrationSiteAddressDto>()
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.NationId : 0))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.GridReference, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? src.ReprocessingSiteAddress.GridReference : string.Empty))
            .ForMember(dest => dest.LegalCorrespondenceAddress, opt => opt.MapFrom(src => src.LegalDocumentAddress != null ? CreateAddressString(src.LegalDocumentAddress) : string.Empty))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.OrganisationId))
            .ForMember(dest => dest.RegulatorRegistrationTaskStatusId, opt => opt.MapFrom(src => GetRegistrationTaskExternalId(src.Tasks, RegulatorTaskNames.SiteAddressAndContactDetails)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetRegistrationTaskStatus(src.Tasks, RegulatorTaskNames.SiteAddressAndContactDetails)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetRegistrationTaskNotes(src.Tasks, RegulatorTaskNames.SiteAddressAndContactDetails)));
    }

    private void CreateCarrierMappings()
    {
        CreateMap<Registration, RegistrationWasteCarrierDto>()
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.WasteCarrierBrokerDealerNumber, opt => opt.MapFrom(src => src.CarrierBrokerDealerPermit != null ? src.CarrierBrokerDealerPermit.WasteCarrierBrokerDealerRegistration : null))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.OrganisationId))
            .ForMember(dest => dest.RegulatorRegistrationTaskStatusId, opt => opt.MapFrom(src => GetRegistrationTaskExternalId(src.Tasks, RegulatorTaskNames.WasteCarrierBrokerDealerNumber)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetRegistrationTaskStatus(src.Tasks, RegulatorTaskNames.WasteCarrierBrokerDealerNumber)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetRegistrationTaskNotes(src.Tasks, RegulatorTaskNames.WasteCarrierBrokerDealerNumber)));
    }

    private void CreateMaterialsAuthorisedMappings()
    {
        CreateMap<Registration, MaterialsAuthorisedOnSiteDto>()
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.OrganisationId))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.ReprocessingSiteAddress != null ? CreateAddressString(src.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.MaterialsAuthorisation, opt => opt.MapFrom(src => src.Materials))
            .ForMember(dest => dest.RegulatorRegistrationTaskStatusId, opt => opt.MapFrom(src => GetRegistrationTaskExternalId(src.Tasks, RegulatorTaskNames.MaterialsAuthorisedOnSite)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetRegistrationTaskStatus(src.Tasks, RegulatorTaskNames.MaterialsAuthorisedOnSite)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetRegistrationTaskNotes(src.Tasks, RegulatorTaskNames.MaterialsAuthorisedOnSite)));

        CreateMap<RegistrationMaterial, MaterialsAuthorisedOnSiteInfoDto>()
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.IsMaterialRegistered == false ? src.ReasonforNotreg : string.Empty));
    }

    private void CreateMaterialPaymentFeeMappings()
    {
        CreateMap<RegistrationMaterial, MaterialPaymentFeeDto>()
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.Registration.OrganisationId))
            .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => (ApplicationOrganisationType)src.Registration.ApplicationTypeId))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.SiteAddress, opt => opt.MapFrom(src => src.Registration.ReprocessingSiteAddress != null ? CreateAddressString(src.Registration.ReprocessingSiteAddress) : string.Empty))
            .ForMember(dest => dest.NationId, opt => opt.MapFrom(src => GetNationId(src.Registration)))
            .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
            .ForMember(dest => dest.DeterminationDate, opt => opt.MapFrom(src => src.DeterminationDate != null ? src.DeterminationDate.DeterminateDate : (DateTime?)null))
            .ForMember(dest => dest.DulyMadeDate, opt => opt.MapFrom(src => src.DulyMade!.DulyMadeDate))
            .ForMember(dest => dest.RegulatorApplicationTaskStatusId, opt => opt.MapFrom(src => GetApplicationTaskExternalId(src.Tasks, RegulatorTaskNames.CheckRegistrationStatus)))
            .ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => GetApplicationTaskStatus(src.Tasks, RegulatorTaskNames.CheckRegistrationStatus)))
            .ForMember(dest => dest.QueryNotes, opt => opt.MapFrom(src => GetApplicationTaskNotes(src.Tasks, RegulatorTaskNames.CheckRegistrationStatus)));
    }

    private void CreateDtoToEntityMappings()
    {
        CreateMap<RegistrationMaterialDto, RegistrationMaterial>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationId, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.DeterminationDate, opt => opt.Ignore())
            .ForMember(dest => dest.Accreditations, opt => opt.Ignore())
            .ForMember(dest => dest.MaterialId, opt => opt.MapFrom(src => src.MaterialId))
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusId))
            .ForMember(dest => dest.StatusUpdatedDate, opt => opt.MapFrom(src => src.StatusUpdatedDate ?? DateTime.UtcNow))
            .ForMember(dest => dest.PermitTypeId, opt => opt.MapFrom(src => src.PermitTypeId))
            .ForMember(dest => dest.PPCReprocessingCapacityTonne, opt => opt.MapFrom(src => src.PPCReprocessingCapacityTonne))
            .ForMember(dest => dest.WasteManagementReprocessingCapacityTonne, opt => opt.MapFrom(src => src.WasteManagementReprocessingCapacityTonne))
            .ForMember(dest => dest.InstallationReprocessingTonne, opt => opt.MapFrom(src => src.InstallationReprocessingTonne))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementTonne, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementTonne))
            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate));

        CreateMap<RegistrationMaterial, ApplicantRegistrationMaterialDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.PPCReprocessingCapacityTonne, opt => opt.MapFrom(src => src.PPCReprocessingCapacityTonne))
            .ForMember(dest => dest.WasteManagementReprocessingCapacityTonne, opt => opt.MapFrom(src => src.WasteManagementReprocessingCapacityTonne))
            .ForMember(dest => dest.InstallationReprocessingTonne, opt => opt.MapFrom(src => src.InstallationReprocessingTonne))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementTonne, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementTonne))
            .ForMember(dest => dest.MaximumReprocessingCapacityTonne, opt => opt.MapFrom(src => src.MaximumReprocessingCapacityTonne))
            .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered))
            .ForMember(dest => dest.PPCPermitNumber, opt => opt.MapFrom(src => src.PPCPermitNumber))
            .ForMember(dest => dest.InstallationPermitNumber, opt => opt.MapFrom(src => src.InstallationPermitNumber))
            .ForMember(dest => dest.EnvironmentalPermitWasteManagementNumber, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementNumber))
            .ForMember(dest => dest.WasteManagementLicenceNumber, opt => opt.MapFrom(src => src.WasteManagementLicenceNumber))
            .ForMember(dest => dest.MaterialLookup, opt => opt.MapFrom(src => src.Material))
            .ForMember(dest => dest.StatusLookup, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.PermitType, opt => opt.MapFrom(src => src.PermitType))
            .ForMember(dest => dest.ExemptionReferences, opt => opt.MapFrom(src => src.MaterialExemptionReferences))
            .ForMember(dest => dest.PPCPeriodId, opt => opt.MapFrom(src => src.PPCPeriodId))
            .ForMember(dest => dest.InstallationPeriodId, opt => opt.MapFrom(src => src.InstallationPeriodId))
            .ForMember(dest => dest.WasteManagementPeriodId, opt => opt.MapFrom(src => src.WasteManagementPeriodId))
            .ForMember(dest => dest.EnvironmentalPeriodId, opt => opt.MapFrom(src => src.EnvironmentalPermitWasteManagementPeriodId));
    }

    private void CreateLookupMappings()
    {
        CreateMap<MaterialExemptionReference, ExemptionReferencesLookupDto>()
            .ForMember(dest => dest.ReferenceNumber, opt => opt.MapFrom(src => src.ReferenceNo));

        CreateMap<LookupMaterial, MaterialLookupDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.MaterialName));

        CreateMap<LookupMaterialPermit, PermitTypeLookupDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<LookupRegistrationMaterialStatus, MaterialStatusLookupDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Name));
    }

    private void CreateRegistrationMaterialContactMappings()
    {
        CreateMap<RegistrationMaterialContact, RegistrationMaterialContactDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId));
    }

    private List<QueryNoteDto> GetRegistrationTaskNotes(List<RegulatorRegistrationTaskStatus>? srcTasks, string taskName)
    {
        if (srcTasks == null)
        {
            return [];
        }

        return srcTasks
            .Where(t => t.Task.Name == taskName)
            .SelectMany(task => task.RegistrationTaskStatusQueryNotes)
            .Select(noteLink => noteLink.QueryNote)
            .Distinct()
            .Select(qn => new QueryNoteDto
            {
                Notes = qn.Notes,
                CreatedBy = qn.CreatedBy,
                CreatedDate = qn.CreatedDate
            }).ToList();
    }

    private Guid? GetRegistrationTaskExternalId(List<RegulatorRegistrationTaskStatus>? srcTasks, string taskName)
    {
        var task = srcTasks?.FirstOrDefault(t => t.Task.Name == taskName);

        return task?.ExternalId;
    }

    private RegulatorTaskStatus GetRegistrationTaskStatus(List<RegulatorRegistrationTaskStatus>? srcTasks, string taskName)
    {
        var task = srcTasks?.FirstOrDefault(t => t.Task.Name == taskName);

        if (task != null)
        {
            return (RegulatorTaskStatus)task.TaskStatusId;
        }

        return RegulatorTaskStatus.NotStarted;
    }

    private List<QueryNoteDto> GetApplicationTaskNotes(List<RegulatorApplicationTaskStatus>? srcTasks, string taskName)
    {
        if (srcTasks == null)
        {
            return [];
        }

        return srcTasks
            .Where(t => t.Task.Name == taskName)
            .SelectMany(task => task.ApplicationTaskStatusQueryNotes)
            .Select(noteLink => noteLink.Note)
            .Distinct()
            .Select(qn => new QueryNoteDto
            {
                Notes = qn.Notes,
                CreatedBy = qn.CreatedBy,
                CreatedDate = qn.CreatedDate
            }).ToList();
    }

    private Guid? GetApplicationTaskExternalId(List<RegulatorApplicationTaskStatus>? srcTasks, string taskName)
    {
        var task = srcTasks?.FirstOrDefault(t => t.Task.Name == taskName);

        return task?.ExternalId;
    }

    private RegulatorTaskStatus GetApplicationTaskStatus(List<RegulatorApplicationTaskStatus>? srcTasks, string taskName)
    {
        var task = srcTasks?.FirstOrDefault(t => t.Task.Name == taskName);

        if (task != null)
        {
            return (RegulatorTaskStatus)task.TaskStatusId;
        }

        return RegulatorTaskStatus.NotStarted;
    }

    private RegulatorTaskStatus GetAccreditationTaskStatus(List<RegulatorAccreditationTaskStatus>? srcTasks, string taskName)
    {
        var task = srcTasks?.FirstOrDefault(t => t.Task.Name == taskName);

        if (task != null)
        {
            return (RegulatorTaskStatus)task.TaskStatusId;
        }

        return RegulatorTaskStatus.NotStarted;
    }

    private static List<RegistrationTaskDto> MapTasks(List<RegulatorRegistrationTaskStatus>? registrationTasks, List<RegulatorAccreditationRegistrationTaskStatus>? accreditationTasks, ResolutionContext context)
    {
        var registrationTasksDto = context.Mapper.Map<List<RegistrationTaskDto>>(registrationTasks);
        var accreditationTasksDto = context.Mapper.Map<List<RegistrationTaskDto>>(accreditationTasks);

        registrationTasksDto.AddRange(accreditationTasksDto);

        return registrationTasksDto;
    }

    private static int GetNationId(Registration registration)
    {
        if (registration.ApplicationTypeId == (int)ApplicationOrganisationType.Reprocessor)
        {
            return registration.ReprocessingSiteAddress?.NationId ?? 0;
        }
        return registration.BusinessAddress?.NationId ?? 0;
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


    private void CreateOverseasReprocessingMappings()
    {
        CreateMap<OverseasMaterialReprocessingSite, OverseasMaterialReprocessingSiteDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OverseasAddressId, opt => opt.MapFrom(src => src.OverseasAddress!.ExternalId))
            .ForMember(dest => dest.OverseasAddress, opt => opt.MapFrom(src => src.OverseasAddress))
            .ForMember(dest => dest.InterimSiteAddresses, opt => opt.Ignore()); // Populate manually in handler

        CreateMap<OverseasAddress, OverseasAddressDto>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationName))
            .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
            .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2))
            .ForMember(dest => dest.CityOrTown, opt => opt.MapFrom(src => src.CityOrTown))
            .ForMember(dest => dest.StateProvince, opt => opt.MapFrom(src => src.StateProvince))
            .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.SiteCoordinates, opt => opt.MapFrom(src => src.SiteCoordinates))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country!.Name))
            .ForMember(dest => dest.OverseasAddressContacts, opt => opt.MapFrom(src => src.OverseasAddressContacts))
            .ForMember(dest => dest.OverseasAddressWasteCodes, opt => opt.MapFrom(src => src.OverseasAddressWasteCodes));

        CreateMap<OverseasAddressContact, OverseasAddressContactDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));

        CreateMap<OverseasAddressWasteCode, OverseasAddressWasteCodeDto>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.CodeName, opt => opt.MapFrom(src => src.CodeName));

        CreateMap<OverseasAddress, InterimSiteAddressDto>()
            .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(src => src.OrganisationName))
            .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.AddressLine1))
            .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.AddressLine2 ?? string.Empty))
            .ForMember(dest => dest.CityOrTown, opt => opt.MapFrom(src => src.CityOrTown))
            .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country!.Name))
            .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode ?? string.Empty))
            .ForMember(dest => dest.StateProvince, opt => opt.MapFrom(src => src.StateProvince ?? string.Empty))
            .ForMember(dest => dest.InterimAddressContact, opt => opt.MapFrom(src => src.OverseasAddressContacts));
    }
}