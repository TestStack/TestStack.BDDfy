using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestStack.BDDfy.Reporters.Serializers
{
    public class CustomJsonSerializer : ISerializer
    {
        private class TypeToStringConverter: JsonConverter<Type>
        {
            public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
            public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options) => writer.WriteStringValue(value.FullName);
        }

        private class ExceptionConverter: JsonConverter<Exception>
        {
            public override Exception Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => throw new NotImplementedException();
            public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options) => writer.WriteStringValue(value.Message);
        }

        private static readonly JsonSerializerOptions SerializerOptionsDefault = new(JsonSerializerDefaults.General)
        {
            WriteIndented = true,
            Converters = { new TypeToStringConverter() , new JsonStringEnumConverter(), new ExceptionConverter() }
        };

        public string Serialize(object obj)
        {
            try
            {
                return JsonSerializer.Serialize(obj, SerializerOptionsDefault);
            }
            catch (SerializationException ex)
            {
                return $"Serialization failed: {ex.Message}";
            }
        }
    }
}