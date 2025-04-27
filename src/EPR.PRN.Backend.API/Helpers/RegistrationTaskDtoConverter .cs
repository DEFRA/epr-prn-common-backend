using EPR.PRN.Backend.API.Dto.Regulator;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RegistrationTaskDtoConverter : JsonConverter<RegistrationTaskDto>
{
    public override RegistrationTaskDto? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException(); // Only needed for deserialization if needed
    }

    public override void Write(Utf8JsonWriter writer, RegistrationTaskDto value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteNumber("id", value.Id ?? 0);
        writer.WriteString("taskName", value.TaskName);
        writer.WriteString("status", value.Status);

        if (value.TaskData != null)
        {
            var taskDataTypeName = value.TaskData.GetType().Name;

            string dynamicPropertyName = taskDataTypeName switch
            {
                nameof(SiteAddressAndContactDetailsTaskDataDto) => "siteAddressAndContactDetails",
                nameof(MaterialsAuthorisedOnSiteTaskDataDto) => "materialsAuthorisedOnSite",
                _ => "taskData"
            };

            writer.WritePropertyName(dynamicPropertyName);
            JsonSerializer.Serialize(writer, value.TaskData, value.TaskData.GetType(), options);
        }

        writer.WriteEndObject();
    }
}
