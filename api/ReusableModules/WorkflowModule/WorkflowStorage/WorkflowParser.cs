using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using WorkflowModule.Descriptors;

namespace WorkflowModule.WorkflowStorage
{
    public class WorkflowParser
    {
        public WorkflowDescriptor GetWorkflowDescriptor(string yamlContent)
        {
            var deserializer = new DeserializerBuilder()
                                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                    .Build();
            var workflowDescriptor = deserializer.Deserialize<WorkflowDescriptor>(yamlContent);

            return workflowDescriptor;
        }
    }
}
