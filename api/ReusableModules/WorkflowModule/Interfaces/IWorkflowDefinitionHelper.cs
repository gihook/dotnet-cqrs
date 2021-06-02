using WorkflowModule.Descriptors;
using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IWorkflowDefinitionHelper
    {
        bool EventIsAllowed(EventDataWithState eventDataWithState);
        EventDescriptor GetEventDescriptor(EventDataWithState eventDataWithState);
        EventTransitionDescriptor GetMatchingEventTransitionDescriptor(StateInfo stateInfo, string eventName);
    }
}
