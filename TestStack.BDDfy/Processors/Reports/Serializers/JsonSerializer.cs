using System.Web.Script.Serialization;

namespace TestStack.BDDfy.Processors.Reports.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(obj);

            return new JsonFormatter(json).Format();
        }
    }
}