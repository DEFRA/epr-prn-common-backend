using AutoMapper;
using EPR.PRN.Backend.API.Dto;
using EPR.PRN.Backend.Data.DataModels.Registrations;

namespace EPR.PRN.Backend.API.Profiles.Regulator;

public class RegistrationTaskOverviewProfile : Profile
{
    public RegistrationTaskOverviewProfile()
    { 
        CreateTaskMappings();
        CreateMap<ApplicantRegistrationTaskStatus, ApplicantRegistrationTaskDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Task.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.TaskStatus.Name));
    }
    
    private void CreateTaskMappings()
    {
        CreateMap<Registration, ApplicantRegistrationTasksOverviewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.OrganisationId, opt => opt.MapFrom(src => src.OrganisationId))
            .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials ?? new List<RegistrationMaterial>()))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom((src, dest, _, context) => MapTasks(src.ApplicantRegistrationTasksStatus, context)));

        CreateMap<RegistrationMaterial, ApplicantRegistrationMaterialTaskOverviewDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalId))
            .ForMember(dest => dest.RegistrationId, opt => opt.MapFrom(src => src.Registration.ExternalId))
            .ForMember(dest => dest.MaterialLookup, opt => opt.MapFrom(src => src.Material))
            .ForMember(dest => dest.StatusLookup, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsMaterialRegistered, opt => opt.MapFrom(src => src.IsMaterialRegistered))
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom((src, dest, _, context) => MapRegistrationMaterialTasks(src.ApplicantTaskStatuses, context)));
    }
    
    private static List<ApplicantRegistrationTaskDto> MapTasks(List<ApplicantRegistrationTaskStatus>? applicantRegistrationTasks, ResolutionContext context) 
        => context.Mapper.Map<List<ApplicantRegistrationTaskDto>>(applicantRegistrationTasks);

    private static List<ApplicantRegistrationTaskDto> MapRegistrationMaterialTasks(List<ApplicantRegistrationTaskStatus>? applicantRegistrationMaterialTasks, ResolutionContext context) 
        => context.Mapper.Map<List<ApplicantRegistrationTaskDto>>(applicantRegistrationMaterialTasks);
}