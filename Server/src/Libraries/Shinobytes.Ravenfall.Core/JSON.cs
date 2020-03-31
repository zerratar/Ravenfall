using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shinobytes.Ravenfall.Core
{
    public static class JSON
    {
        public static T Parse<T>(string data)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });
        }

        public static string Stringify(object obj)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            });
        }
    }
}