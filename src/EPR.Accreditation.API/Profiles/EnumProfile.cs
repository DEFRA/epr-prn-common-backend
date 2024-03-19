using AutoMapper;
using Data = EPR.Accreditation.API.Common.Data.Enums;
using DTO = EPR.Accreditation.API.Common.Enums;

namespace EPR.Accreditation.API.Profiles
{
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<Data.AccreditationStatus, DTO.AccreditationStatus>();
            CreateMap<Data.FileUploadStatus, DTO.FileUploadStatus>();
            CreateMap<Data.FileUploadType, DTO.FileUploadType>();
            CreateMap<Data.OperatorType, DTO.OperatorType>();
            CreateMap<Data.ReprocessorSupportingInformationType, DTO.ReprocessorSupportingInformationType>();
            CreateMap<Data.SupportingInformationType, DTO.SupportingInformationType>();
            CreateMap<Data.TaskName, DTO.TaskName>();
            CreateMap<Data.TaskStatus, DTO.TaskStatus>();
            CreateMap<Data.WasteCodeType, DTO.WasteCodeType>();
        }
    }
}
