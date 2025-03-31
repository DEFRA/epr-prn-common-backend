//public class Mapper : IMapper
//{
//    public TDestination Map<TDestination>(RegistrationTaskStatusDto dto)
//    {
//        if (dto == null) throw new ArgumentNullException(nameof(dto));

//        if (typeof(TDestination) == typeof(UpdateRegulatorApplicationTaskCommand))
//        {
//            return (TDestination)(object)new UpdateRegulatorApplicationTaskCommand
//            {
//                Status = dto.Status,
//                Comment = dto.Comment
//            };
//        }

//        if (typeof(TDestination) == typeof(UpdateRegulatorRegistrationTaskCommand))
//        {
//            return (TDestination)(object)new UpdateRegulatorRegistrationTaskCommand
//            {
//                Status = dto.Status,
//                Comment = dto.Comment
//            };
//        }

//        throw new InvalidOperationException($"Mapping not configured for type {typeof(TDestination)}");
//    }
//}