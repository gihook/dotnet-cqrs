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
                                    .WithAttributeOverride<WorkflowDescriptor>(
                                            wd => wd.EventDescriptors,
                                            new YamlMemberAttribute()
                                            {
                                                Alias = "events"
                                            })
                                    .WithAttributeOverride<EventDescriptor>(
                                            x => x.ReducerDescriptor,
                                            new YamlMemberAttribute()
                                            {
                                                Alias = "reducer"
                                            })
                                    .WithAttributeOverride<EventDescriptor>(
                                            x => x.ValidatorDescriptors,
                                            new YamlMemberAttribute()
                                            {
                                                Alias = "validators"
                                            })
                                    .Build();
            var workflowDescriptor = deserializer.Deserialize<WorkflowDescriptor>(yamlContent);

            return workflowDescriptor;
        }
    }
}
