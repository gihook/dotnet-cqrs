using System.Collections.Generic;

namespace WorkflowModule.Descriptors
{
    public class EventTransitionDescriptor
    {
        public string Event { get; set; }
        public string FromState { get; set; }
        public IEnumerable<ConditionalTransition> ConditionalTransitions { get; set; }
    }
}
