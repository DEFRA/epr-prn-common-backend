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




        CreateMap<Accreditation, AccreditationDto>()
          .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))


    // Example: Map nested MaterialName
    .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.RegistrationMaterial.Material.MaterialName))
    // Example: Map AccreditationStatusName from nested AccreditationStatus.Name
    .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatus.Id))
    // Example: Map ApplicationTypeName from nested ApplicationType.Name
    .ForMember(dest => dest.ApplicationTypeId, opt => opt.MapFrom(src => src.ApplicationType.Id))
    // Example: Map RegistrationMaterialId if property names differ
    .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
    // Example: Map CreatedDate if property names differ
    //  .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedOn))
    // Add more custom mappings as needed for other non-identical but related members
    .ForMember(des => des.AccreferenceNumber, opt => opt.MapFrom(src => src.ApplicationReferenceNumber))
    ;

        CreateMap<AccreditationRequestDto, Accreditation>()
    // Example: Map ApplicationTypeId if property names differ
    .ForMember(dest => dest.ApplicationTypeId, opt => opt.MapFrom(src => src.ApplicationTypeId))
    // Example: Map AccreditationStatusId if property names differ
    .ForMember(dest => dest.AccreditationStatusId, opt => opt.MapFrom(src => src.AccreditationStatusId))
    // Example: Map RegistrationMaterialId if property names differ
    .ForMember(dest => dest.RegistrationMaterialId, opt => opt.MapFrom(src => src.RegistrationMaterialId))
    // Example: Map ApplicationReferenceNumber if property names differ
    .ForMember(dest => dest.ApplicationReferenceNumber, opt => opt.MapFrom(src => src.AccreferenceNumber))
    // Example: Map CreatedOn if property names differ
    //.ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedDate))
    // Add more custom mappings as needed for other non-identical but related members
    ;


        CreateMap<Data.DataModels.Registrations.AccreditationPrnIssueAuth, AccreditationPrnIssueAuthDto>();
        CreateMap<AccreditationPrnIssueAuthRequestDto, Data.DataModels.Registrations.AccreditationPrnIssueAuth>();
    }
}
/*
 * 
 * Invalid column name 'ApplicationTypeId'.
Invalid column name 'CreatedBy'.
Invalid column name 'DecFullName'.
Invalid column name 'JobTitle'.
Invalid column name 'OrganisationId'.
Invalid column name 'UpdatedBy'.
Invalid column name 'UpdatedOn'.
 * 
 */