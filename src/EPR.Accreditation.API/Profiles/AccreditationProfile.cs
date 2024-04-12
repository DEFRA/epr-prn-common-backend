using AutoMapper;
using EPR.Accreditation.API.Helpers;
using Data = EPR.Accreditation.API.Common.Data.DataModels;
using DTO = EPR.Accreditation.API.Common.Dtos;
using Enums = EPR.Accreditation.API.Common.Data.Enums;

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
                .ForMember(d => d.ExemptionReferences, opt => opt.MapFrom(src => src.ExemptionReferences.Select(er => er.Reference)));

            CreateMap<DTO.Site, Data.Site>()
                .ForMember(d =>
                    d.ExemptionReferences,
                    opt => opt.MapFrom(src =>
                        src.ExemptionReferences.Select(er =>
                            new Data.ExemptionReference
                            {
                                Reference = er
                            })));

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
                .MapOnlyNonDefault()
                .ForMember(d => 
                    d.ReprocessorSupportingInformation, o => 
                    o.MapFrom((s, d, m, context) =>
                    {
                        // if there is source no list, return the original destination list
                        if (s.ReprocessorSupportingInformation.FirstOrDefault() == null)
                            return d.ReprocessorSupportingInformation;

                        // get distinct types from the incoming list.
                        var sourceTypes = context.Mapper.Map<List<Enums.ReprocessorSupportingInformationType>>
                            (s.ReprocessorSupportingInformation
                            .Select(r => r.ReprocessorSupportingInformationTypeId)
                            .Distinct()
                            .ToList());

                        var list = new List<Data.ReprocessorSupportingInformation>();

                        // loop through the source types and add the items that are not the source
                        // type to the list and then add the source list
                        // this ensures that if there is only one incoming type we get a list that
                        // comprises both types still and the data gets updated as we would expect
                        foreach (var sourceType in sourceTypes)
                        {
                            if (d.ReprocessorSupportingInformation != null)
                            {
                                // get the source type - we are
                                list.AddRange(d
                                    .ReprocessorSupportingInformation
                                    .Where(d =>
                                        d.ReprocessorSupportingInformationTypeId != sourceType)
                                    .ToList());
                            }

                            list.AddRange(context.Mapper.Map<List<Data.ReprocessorSupportingInformation>>(
                                s.ReprocessorSupportingInformation.Where(r => 
                                    r.ReprocessorSupportingInformationTypeId == context.Mapper.Map<Common.Enums.ReprocessorSupportingInformationType>(sourceType))));
                        }
                        return list;
                    }));

            CreateMap<Data.ReprocessorSupportingInformation, DTO.ReprocessorSupportingInformation>()
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
