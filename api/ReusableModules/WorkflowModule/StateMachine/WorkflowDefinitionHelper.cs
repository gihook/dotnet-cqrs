using System;
using WorkflowModule.Descriptors;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelper
    {
        public virtual bool EventIsAllowed(EventDataWithState eventDataWithState)
        {
            return false;
        }

        public virtual EventDescriptor GetEventDescriptor(string eventName, string workflowId)
        {
            throw new NotImplementedException();
        }
    }
}
