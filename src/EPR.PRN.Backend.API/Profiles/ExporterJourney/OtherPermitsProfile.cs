using AutoMapper;
using EPR.PRN.Backend.API.Dto.ExporterJourney;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles.Regulator;

public class OtherPermitsProfile : Profile
{
    public OtherPermitsProfile()
    {
        CreateMap<CarrierBrokerDealerPermits, GetCarrierBrokerDealerPermitsResultDto>()
            .ForMember(dest => dest.CarrierBrokerDealerPermitId, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.RegistrationId))
            .ForMember(dest => dest.WasteLicenseOrPermitNumber, opt => opt.MapFrom(src => src.WasteManagementEnvironmentPermitNumber))
            .ForMember(dest => dest.PpcNumber, opt => opt.MapFrom(src => src.InstallationPermitOrPPCNumber))
            .ForMember(dest => dest.WasteExemptionReference, opt => opt.MapFrom(src => CreateWasteExemptionReferenceList(src.WasteExemptionReference)));
    }

    private static List<string> CreateWasteExemptionReferenceList(string? wasteExemptionReference)
    {
        return !string.IsNullOrEmpty(wasteExemptionReference)
            ? wasteExemptionReference.Split(',').ToList()
            : new List<string>();
    }
}