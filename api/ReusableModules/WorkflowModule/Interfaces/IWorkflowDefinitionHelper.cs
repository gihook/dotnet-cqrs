using WorkflowModule.Descriptors;
using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IWorkflowDefinitionHelper
    {
        bool EventIsAllowed(EventDataWithState eventDataWithState);
        EventDescriptor GetEventDescriptor(EventDataWithState eventDataWithState);
        ConditionalTransition GetMatchingTransition(StateInfo stateInfo, string eventName);
    }
}
