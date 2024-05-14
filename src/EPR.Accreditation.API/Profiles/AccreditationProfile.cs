namespace EPR.Accreditation.API.Profiles
{
    using AutoMapper;
    using EPR.Accreditation.API.Helpers.Comparers;
    using Data = EPR.Accreditation.API.Common.Data.DataModels;
    using DTO = EPR.Accreditation.API.Common.Dtos;
    using Enums = EPR.Accreditation.API.Common.Data.Enums;

    public class AccreditationProfile : Profile
    {
        private readonly ReprocessorSupportingInformationDtoComparer _reprocessorSupportingInformationDtoComparer = new ReprocessorSupportingInformationDtoComparer();
        private readonly ReprocessorSupportingInformationDataComparer _reprocessorSupportingInformationDataComparer = new ReprocessorSupportingInformationDataComparer();

        private readonly WasteCodeDtoComparer _wasteCodeDtoComparer = new WasteCodeDtoComparer();
        private readonly WasteCodeDataComparer _wasteCodeDataComparer = new WasteCodeDataComparer();

        public AccreditationProfile()
        {
            CreateMap<Data.Accreditation, DTO.Accreditation>()
                .ReverseMap();

            CreateMap<Data.OverseasReprocessingSite, DTO.OverseasReprocessingSite>()
                .ReverseMap();

            CreateMap<Data.OverseasContactPerson, DTO.OverseasContactPerson>()
                .ReverseMap();

            CreateMap<Data.OverseasAddress, DTO.OverseasAddress>()
                .ReverseMap();

            CreateMap<Data.Site, DTO.Site>()
                .ForMember(d =>
                    d.ExemptionReferences,
                    opt =>
                        opt.MapFrom(src =>
                            src.ExemptionReferences.Select(er => er.Reference)));

            CreateMap<DTO.Site, Data.Site>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ExternalId, o => o.Ignore())
                .ForMember(d => d.AddressId, o => o.Ignore())
                .ForMember(d => d.Accreditations, o => o.Ignore())
                .ForMember(d => d.AccreditationMaterials, o => o.Ignore())
                .ForMember(d =>
                    d.ExemptionReferences,
                    opt => opt.MapFrom(src =>
                        src.ExemptionReferences.Select(er =>
                            new Data.ExemptionReference
                            {
                                Reference = er
                            })));

            CreateMap<Data.SiteAuthority, DTO.SiteAuthority>()
                .ReverseMap();

            CreateMap<Data.FileUpload, DTO.FileUpload>()
                .ReverseMap();

            CreateMap<Data.AccreditationMaterial, DTO.Response.AccreditationMaterial>();

            CreateMap<DTO.Request.AccreditationMaterial, Data.AccreditationMaterial>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ExternalId, o => o.Ignore())
                .ForMember(d => d.AccreditationId, o => o.Ignore())
                .ForMember(d => d.Accreditation, o => o.Ignore())
                .ForMember(d => d.SiteId, o => o.Ignore())
                .ForMember(d => d.Site, o => o.Ignore())
                .ForMember(d => d.OverseasReprocessingSiteId, o => o.Ignore())
                .ForMember(d => d.OverseasReprocessingSite, o => o.Ignore())
                .ForMember(d => d.AccreditationTaskProgressMaterials, o => o.Ignore())
                .ForMember(d => d.Material, o => o.Ignore()) // do not want to map incoming materials
                .ForMember(d => d.MaterialId, o => o.Ignore()) // material id does not exist on DTOs, so don't map
                .ForMember(d =>
                    // Mapping of WasteCodes is complicated as it needs to represent two versions of data,
                    // and an update may just include one set of data. So we need to carefully manage the merging of the data
                    // We also want to ensure that as little updates to the database occur, so need to ensure we work with
                    // the destination list
                    d.WasteCodes, 
                    o =>
                        o.MapFrom((s, d, m, context) =>
                        {
                            // if incoming list is null, return original list
                            // for some reason just returning the original list results in the assigning of an empty list
                            // re-mapping (presumably creating a new version) fixes this issue
                            if (s.WasteCodes == null ||
                                !s.WasteCodes.Any())
                            {
                                return context.Mapper.Map<IList<Data.WasteCode>>(d.WasteCodes);
                            }

                            if (d.WasteCodes == null)
                            {
                                d.WasteCodes = new List<Data.WasteCode>();
                            }

                            // get the incoming source list type
                            var sourceType = context.Mapper.Map<Enums.WasteCodeType>(s.WasteCodes.First().WasteCodeTypeId);

                            // loop through to see if any entries need removing
                            var destList = d.WasteCodes.ToList();

                            for (int i = d.WasteCodes.Count - 1; i >= 0; i--)
                            {
                                var item = destList[i];
                                if (item.WasteCodeTypeId != sourceType)
                                    continue;

                                if (!s.WasteCodes.Contains(
                                    new DTO.WasteCode
                                    {
                                        Code = item.Code
                                    },
                                    _wasteCodeDtoComparer))
                                {
                                    destList.RemoveAt(i);
                                }
                            }

                            d.WasteCodes = destList;

                            // loop through to see if any entries need adding
                            foreach (var item in s.WasteCodes)
                            {
                                var newDataItem = new Data.WasteCode
                                {
                                    WasteCodeTypeId = sourceType,
                                    Code = item.Code
                                };

                                if (!d.WasteCodes.Contains(
                                    newDataItem,
                                    _wasteCodeDataComparer))
                                {
                                    d.WasteCodes.Add(newDataItem);
                                }
                            }

                            return context.Mapper.Map<IList<Data.WasteCode>>(d.WasteCodes);
                        }
                ));

            CreateMap<Data.WasteCode, DTO.WasteCode>()
                .ReverseMap();

            CreateMap<Data.AccreditationTaskProgress, DTO.AccreditationTaskProgress>()
                .ReverseMap();

            CreateMap<Data.MaterialReprocessorDetails, DTO.MaterialReprocessorDetails>()
                .ReverseMap()
                .ForMember(d =>
                    // Mapping of ReprocessorSupportingInformation is complicated as it needs to represent two versions of data,
                    // and an update may just include one set of data. So we need to carefully manage the merging of the data
                    // We also want to ensure that as little updates to the database occur, so need to ensure we work with
                    // the destination list
                    d.ReprocessorSupportingInformation,
                    o =>
                        o.MapFrom((s, d, m, context) =>
                        {
                            // if incoming list is null, return original list
                            // for some reason just returning the original list results in the assigning of an empty list
                            // re-mapping (presumably creating a new version) fixes this issue
                            if (s.ReprocessorSupportingInformation == null ||
                                !s.ReprocessorSupportingInformation.Any())
                            {
                                return context.Mapper.Map<IList<Data.ReprocessorSupportingInformation>>(d.ReprocessorSupportingInformation);
                            }

                            if (d.ReprocessorSupportingInformation == null)
                            {
                                d.ReprocessorSupportingInformation = new List<Data.ReprocessorSupportingInformation>();
                            }

                            // get the incoming source list type
                            var sourceType = context.Mapper.Map<Enums.ReprocessorSupportingInformationType>(s.ReprocessorSupportingInformation.First().ReprocessorSupportingInformationTypeId);

                            // loop through to see if any entries need removing
                            foreach (var item in d.ReprocessorSupportingInformation)
                            {
                                if (item.ReprocessorSupportingInformationTypeId != sourceType)
                                    continue;

                                if (!s.ReprocessorSupportingInformation.Contains(
                                    new DTO.ReprocessorSupportingInformation
                                    {
                                        Type = item.Type,
                                        Tonnes = item.Tonnes
                                    },
                                    _reprocessorSupportingInformationDtoComparer))
                                {
                                    d.ReprocessorSupportingInformation.Remove(item);
                                }
                            }

                            // loop through to see if any entries need adding
                            foreach(var item in s.ReprocessorSupportingInformation)
                            {
                                var newDataItem = new Data.ReprocessorSupportingInformation
                                {
                                    ReprocessorSupportingInformationTypeId = sourceType,
                                    Type = item.Type,
                                    Tonnes = item.Tonnes
                                };

                                if (!d.ReprocessorSupportingInformation.Contains(
                                    newDataItem,
                                    _reprocessorSupportingInformationDataComparer))
                                {
                                    d.ReprocessorSupportingInformation.Add(newDataItem);
                                }
                            }

                            return context.Mapper.Map<IList<Data.ReprocessorSupportingInformation>>(d.ReprocessorSupportingInformation);
                        }));

            CreateMap<Data.ReprocessorSupportingInformation, DTO.ReprocessorSupportingInformation>()
                .ReverseMap();

            CreateMap<Data.WastePermit, DTO.WastePermit>()
                .ReverseMap();

            CreateMap<Data.SaveAndComeBack, DTO.SaveAndComeBack>()
                .ReverseMap();

            CreateMap<Data.Lookups.Country, DTO.Country>()
                .ReverseMap();

            CreateMap<Data.Material, DTO.Material>()
                .ReverseMap();

            CreateMap<Data.Address, DTO.Address>()
                .ReverseMap();
        }
    }
}