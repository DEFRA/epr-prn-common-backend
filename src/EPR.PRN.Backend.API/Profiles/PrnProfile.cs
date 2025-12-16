using AutoMapper;
using EPR.PRN.Backend.API.Common.Dto;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels;

namespace EPR.PRN.Backend.API.Profiles;

public class PrnProfile : Profile
{
    public PrnProfile()
    {
        CreateMap<SavePrnDetailsRequestV2, Eprn>();
        CreateMap<Eprn, PrnDto>();
    }

    public static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => cfg.AddProfile<PrnProfile>()).CreateMapper();
    }
}