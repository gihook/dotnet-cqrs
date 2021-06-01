using WorkflowModule.Descriptors;
using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IWorkflowDefinitionHelper
    {
        bool EventIsAllowed(EventDataWithState eventDataWithState);
        EventDescriptor GetEventDescriptor(string eventName, string workflowId);
    }
}
