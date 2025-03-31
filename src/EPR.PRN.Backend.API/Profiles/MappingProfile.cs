using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateTaskStatusRequestDto, UpdateRegulatorApplicationTaskCommand>();
        CreateMap<UpdateTaskStatusRequestDto, UpdateRegulatorRegistrationTaskCommand>();
    }
}