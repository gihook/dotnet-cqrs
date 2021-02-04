using System.Linq;
using Action.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Action.Core
{
    public class YamlActionDescriptorReader
    {
        public ActionInfo CreateActionInfo(string content)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

            var result = deserializer.Deserialize<ActionInfo>(content);
            result.Parameters = result.Parameters.Select(p =>
            {
                p.DisplayName = p.DisplayName ?? p.ParameterName;
                return p;
            });

            return result;
        }
    }
}
