using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace TestStack.BDDfy.Reporters.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            string json;
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                json = Encoding.UTF8.GetString(stream.ToArray());
            }

            return new JsonFormatter(json).Format();
        }
    }
}