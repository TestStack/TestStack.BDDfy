namespace TestStack.BDDfy.Reporters.Serializers
{
#if NET40
    using System.Web.Script.Serialization;

    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(obj);

            return new JsonFormatter(json).Format();
        }
    }

#else
    using Newtonsoft.Json;

    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }

#endif

}