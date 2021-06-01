using System;
using WorkflowModule.Descriptors;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelper : IWorkflowDefinitionHelper
    {
        public bool EventIsAllowed(EventDataWithState eventDataWithState)
        {
            return false;
        }

        public EventDescriptor GetEventDescriptor(string eventName, string workflowId)
        {
            throw new NotImplementedException();
        }
    }
}
