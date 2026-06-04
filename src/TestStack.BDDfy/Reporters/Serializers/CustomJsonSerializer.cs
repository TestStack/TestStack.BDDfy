using System.Text.Json;

namespace TestStack.BDDfy.Reporters.Serializers
{
    public class CustomJsonSerializer : ISerializer
    {
        private static readonly JsonSerializerOptions SerializerOptionsDefault = new(JsonSerializerDefaults.General)
        {
            WriteIndented = true
        };

        public string Serialize(object obj) => JsonSerializer.Serialize(obj, SerializerOptionsDefault);
    }
}