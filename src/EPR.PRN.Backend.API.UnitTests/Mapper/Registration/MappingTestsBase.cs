using AutoMapper;
using EPR.PRN.Backend.API.Profiles.Regulator;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

public interface IMappingTestBase
{
    IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(RegistrationMaterialProfile).Assembly);
        });

        return config.CreateMapper();
    }
}