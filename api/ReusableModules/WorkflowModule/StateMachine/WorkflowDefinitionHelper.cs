using System;
using WorkflowModule.Descriptors;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelper
    {
        public virtual bool EventIsAllowed(string eventName, StateInfo stateInfo, string workflowId)
        {
            return false;
        }

        public virtual EventDescriptor GetEventDescriptor(string eventName, string workflowId)
        {
            throw new NotImplementedException();
        }
    }
}
