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
            workflowDescriptor.EventTransitionDescriptors = ParseEventTransitions(deserializedDescriptor);

            return workflowDescriptor;
        }

        private string ReadStringKey(Dictionary<object, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key) ? (dictionary[key] as string) : String.Empty;
        }

        private Dictionary<object, object> ReadKey(Dictionary<object, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? dictionary[key] as Dictionary<object, object>
                : new Dictionary<object, object>();
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

                var inputsDict = ReadKey(item, "inputs");
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
            var obj = dictionary["reducer"];

            var dict = obj as Dictionary<object, object>;

            var isString = dict == null;

            var type = isString
                           ? obj as string
                           : dict["type"] as string;

            result.Type = type;

            if (!isString)
            {
                result.Params = ParseObjectList<string>(dict, "params");
            }

            return result;
        }

        private IEnumerable<EventTransitionDescriptor> ParseEventTransitions(Dictionary<object, object> dictionary)
        {
            var result = ParseObjectList<Dictionary<object, object>>(dictionary, "eventTransitions");

            return result.Select(ParseEventTransitionItem);
        }

        private EventTransitionDescriptor ParseEventTransitionItem(Dictionary<object, object> item)
        {
            var descriptor = new EventTransitionDescriptor();
            descriptor.Event = ReadStringKey(item, "event");
            descriptor.FromState = ReadStringKey(item, "fromState");
            descriptor.ConditionalTransitions = GetConditionalTransitions(item);

            return descriptor;
        }

        private IEnumerable<ConditionalTransition> GetConditionalTransitions(Dictionary<object, object> item)
        {
            var entries = ParseObjectList<Dictionary<object, object>>(item, "conditionalTransition");
            var transitions = entries.Select(entry =>
            {
                var transition = new ConditionalTransition();
                transition.Condition = ReadStringKey(entry, "condition");
                transition.Params = ParseObjectList<string>(entry, "params");
                transition.ToState = ReadStringKey(entry, "toState");

                return transition;
            });

            var hasConditionalTransitions = transitions.Count() != 0;
            if (hasConditionalTransitions) return transitions;

            var nonConditionalTransition = new ConditionalTransition()
            {
                Condition = "TRUE",
                Params = Enumerable.Empty<string>(),
                ToState = ReadStringKey(item, "toState")
            };

            return transitions.Append(nonConditionalTransition);
        }
    }
}
