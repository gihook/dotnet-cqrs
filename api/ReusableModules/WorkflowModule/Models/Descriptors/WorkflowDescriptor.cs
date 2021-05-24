using System.Collections.Generic;

namespace WorkflowModule.Descriptors
{
    public class WorkflowDescriptor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public IEnumerable<string> States { get; set; }
        public IEnumerable<EventDescriptor> EventDescriptors { get; set; }
        /* public IEnumerable<EventTransitionDescriptor> EventTransitions { get; set; } */
    }
}
