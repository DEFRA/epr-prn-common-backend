using AutoMapper;
using EPR.PRN.Backend.API.Dto.Regulator;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles.Regulator;

public class RegistrationTaskOverviewProfile : Profile
{
    public RegistrationTaskOverviewProfile()
    { 
        CreateTaskMappings();
        CreateMap<ApplicantRegistrationTaskStatus, RegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));
    }
    
    private void CreateTaskMappings()
    {
        CreateMap<Registration, RegistrationTaskOverviewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.Regulator, opt => opt.MapFrom(_ => "EA"))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials ?? new List<RegistrationMaterial>()))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom((src, dest, _, context) => MapTasks(src.ApplicantRegistrationTasksStatus, context)));
    }
    
    private static List<RegistrationTaskDto> MapTasks(List<ApplicantRegistrationTaskStatus>? applicantRegistrationTasks, ResolutionContext context)
    { 
        var registrationTasksDto = context.Mapper.Map<List<RegistrationTaskDto>>(applicantRegistrationTasks);
        return registrationTasksDto;
    }
}