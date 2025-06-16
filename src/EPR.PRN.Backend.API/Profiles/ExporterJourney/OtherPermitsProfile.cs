using AutoMapper;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.ExporterJourney;

namespace EPR.PRN.Backend.API.Profiles.Regulator;

public class OtherPermitsProfile : Profile
{
    public OtherPermitsProfile()
    {
        CreateMap<CarrierBrokerDealerPermit, GetOtherPermitsResultDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.WasteLicenseOrPermitNumber, opt => opt.MapFrom(src => src.WasteManagementorEnvironmentPermitNumber))
            .ForMember(dest => dest.PpcNumber, opt => opt.MapFrom(src => src.InstallationPermitorPPCNumber))
            .ForMember(dest => dest.WasteExemptionReference, opt => opt.MapFrom(src => CreateWasteExemptionReferenceList(src.WasteExemptionReference)));
    }

    private static List<string> CreateWasteExemptionReferenceList(string? wasteExemptionReference)
    {
        if (string.IsNullOrEmpty(wasteExemptionReference))
        {
            return new List<string>();
        }

        return wasteExemptionReference.Split(',').ToList();
    }
}