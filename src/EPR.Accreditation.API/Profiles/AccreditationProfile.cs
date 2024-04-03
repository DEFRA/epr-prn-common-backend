using AutoMapper;
using EPR.Accreditation.API.Helpers;
using Data = EPR.Accreditation.API.Common.Data.DataModels;
using DTO = EPR.Accreditation.API.Common.Dtos;

namespace EPR.Accreditation.API.Profiles
{
    public class AccreditationProfile : Profile
    {
        public AccreditationProfile()
        {
            CreateMap<Data.Accreditation, DTO.Accreditation>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.OverseasReprocessingSite, DTO.OverseasReprocessingSite>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.OverseasContactPerson, DTO.OverseasContactPerson>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.OverseasAddress, DTO.OverseasAddress>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.Site, DTO.Site>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .ForMember(d => d.Accreditations, o => o.Ignore())
                .ForMember(d => d.AccreditationMaterials, o => o.Ignore())
                .MapOnlyNonDefault();

            CreateMap<Data.SiteAuthority, DTO.SiteAuthority>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.FileUpload, DTO.FileUpload>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.AccreditationMaterial, DTO.AccreditationMaterial>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.WasteCode, DTO.WasteCode>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.AccreditationTaskProgress, DTO.AccreditationTaskProgress>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.MaterialReprocessorDetails, DTO.MaterialReprocessorDetails>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.WastePermit, DTO.WastePermit>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();

            CreateMap<Data.SaveAndComeBack, DTO.SaveAndComeBack>()
                .ReverseMap();

            CreateMap<Data.Lookups.Country, DTO.Country>()
                .ReverseMap();

            CreateMap<Data.Material, DTO.Material>()
                .ReverseMap();

            CreateMap<Data.ExemptionReference, DTO.ExemptionReference>()
                .ReverseMap();
        }
    }
}
