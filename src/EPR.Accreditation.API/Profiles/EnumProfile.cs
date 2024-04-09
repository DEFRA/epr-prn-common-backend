using AutoMapper;
using Data = EPR.Accreditation.API.Common.Data.Enums;
using DTO = EPR.Accreditation.API.Common.Enums;

namespace EPR.Accreditation.API.Profiles
{
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<Data.AccreditationStatus, DTO.AccreditationStatus>().ReverseMap();
            CreateMap<Data.FileUploadStatus, DTO.FileUploadStatus>().ReverseMap();
            CreateMap<Data.FileUploadType, DTO.FileUploadType>().ReverseMap();
            CreateMap<Data.OperatorType, DTO.OperatorType>().ReverseMap();
            CreateMap<Data.ReprocessorSupportingInformationType, DTO.ReprocessorSupportingInformationType>().ReverseMap();
            CreateMap<Data.SupportingInformationType, DTO.SupportingInformationType>().ReverseMap();
            CreateMap<Data.TaskName, DTO.TaskName>().ReverseMap();
            CreateMap<Data.TaskStatus, DTO.TaskStatus>().ReverseMap();
            CreateMap<Data.WasteCodeType, DTO.WasteCodeType>().ReverseMap();
            CreateMap<Data.OverseasPersonType, DTO.OverseasPersonType>().ReverseMap();

        }
    }
}
