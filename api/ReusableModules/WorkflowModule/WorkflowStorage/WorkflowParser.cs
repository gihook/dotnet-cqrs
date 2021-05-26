using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using WorkflowModule.Descriptors;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WorkflowModule.WorkflowStorage
{
    public class WorkflowParser
    {
        public WorkflowDescriptor GetWorkflowDescriptor(string yamlContent)
        {
            var deserializer = new DeserializerBuilder()
                                      .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                      .Build();

            var stringReader = new StringReader(yamlContent);
            var deserializedDescriptor = deserializer.Deserialize<Dictionary<object, object>>(stringReader);

            var workflowDescriptor = new WorkflowDescriptor();

            workflowDescriptor.Id = ReadStringKey(deserializedDescriptor, "id");
            workflowDescriptor.Name = ReadStringKey(deserializedDescriptor, "name");
            workflowDescriptor.Description = ReadStringKey(deserializedDescriptor, "description");
            workflowDescriptor.Version = ReadIntKey(deserializedDescriptor, "version");

            workflowDescriptor.States = ParseObjectList<string>(deserializedDescriptor, "states");

            workflowDescriptor.EventDescriptors = ParseEventDescriptors(deserializedDescriptor);

            return workflowDescriptor;
        }

        private string ReadStringKey(Dictionary<object, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key) ? (dictionary[key] as string) : String.Empty;
        }

        private int ReadIntKey(Dictionary<object, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? Int32.Parse(dictionary[key] as string)
                : 0;
        }

        private IEnumerable<T> ParseObjectList<T>(Dictionary<object, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? (dictionary[key] as List<object>).Cast<T>()
                : Enumerable.Empty<T>();
        }

        private IEnumerable<EventDescriptor> ParseEventDescriptors(Dictionary<object, object> dictionary)
        {
            var items = ParseObjectList<Dictionary<object, object>>(dictionary, "events");

            return items.Select(item =>
            {
                var eventDescriptor = new EventDescriptor();
                eventDescriptor.Name = ReadStringKey(item, "name");

                var inputsDict = item["inputs"] as Dictionary<object, object>;
                eventDescriptor.Inputs = ParseInputParameters(inputsDict);

                eventDescriptor.ValidatorDescriptors = ParseInputValidators(item);

                eventDescriptor.ReducerDescriptor = ParseReducer(item);

                return eventDescriptor;
            });
        }

        private IEnumerable<InputValidatorDescriptor> ParseInputValidators(Dictionary<object, object> dictionary)
        {
            var items = ParseObjectList<Dictionary<object, object>>(dictionary, "validators");

            return items.Select(item =>
            {
                var descriptor = new InputValidatorDescriptor();
                descriptor.Type = item["type"] as string;
                descriptor.Params = ParseObjectList<string>(item, "params");

                return descriptor;
            });
        }

        private Dictionary<string, string> ParseInputParameters(Dictionary<object, object> dictionary)
        {
            var result = new Dictionary<string, string>();

            foreach (var kvp in dictionary)
            {
                var key = kvp.Key as string;
                result[key] = kvp.Value as string;
            }

            return result;
        }

        private ReducerDescriptor ParseReducer(Dictionary<object, object> dictionary)
        {
            var result = new ReducerDescriptor();
            result.Type = ReadStringKey(dictionary, "reducer");

            return result;
        }
    }
}
