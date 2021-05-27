using System.Collections.Generic;

namespace WorkflowModule.Descriptors
{
    public class EventDescriptor
    {
        public string Name { get; set; }
        public Dictionary<string, string> Inputs { get; set; }
        public IEnumerable<InputValidatorDescriptor> ValidatorDescriptors { get; set; }
        public ReducerDescriptor ReducerDescriptor { get; set; }
    }
}
