using AutoMapper;

namespace EPR.PRN.Backend.API.UnitTests.Mapper.Registration;

public class MappingTestsBase<T> where T : Profile, new()
{
    // Base class for mapping tests, can contain common setup or utilities for tests
    protected IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<T>());
        return config.CreateMapper();
    }
}