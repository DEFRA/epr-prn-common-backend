using AutoMapper;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Profiles;

public class PrnMapper : Profile
{
    public PrnMapper()
    {
        CreateMap<SavePrnDetailsRequest, Eprn>()
            .ForMember(e => e.IssuerReference, o => o.MapFrom(s => ""));
        CreateMap<Eprn, PrnDto>();
    }

    public static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<PrnMapper>()).CreateMapper();
    }
}
