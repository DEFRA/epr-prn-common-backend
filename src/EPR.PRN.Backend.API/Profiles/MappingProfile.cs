using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationTaskStatusDto, UpdateRegulatorApplicationTaskCommand>();
        CreateMap<RegistrationTaskStatusDto, UpdateRegulatorRegistrationTaskCommand>();
    }
}