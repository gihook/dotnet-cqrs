using System;
using System.Collections.Generic;
using System.Linq;
using WorkflowModule.Descriptors;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelper : IWorkflowDefinitionHelper
    {
        private readonly Dictionary<string, WorkflowDescriptor> _workflowDescriptors;

        public WorkflowDefinitionHelper(IWorkflowDefinitionLoader definitionLoader)
        {
            _workflowDescriptors = definitionLoader.LoadWorkflows();
        }

        public bool EventIsAllowed(EventDataWithState eventDataWithState)
        {
            var workflowId = eventDataWithState.WorkflowId;

            if (!_workflowDescriptors.ContainsKey(workflowId))
            {
                throw new UnknownWorkflowException(workflowId);
            }

            var descriptor = _workflowDescriptors[workflowId];

            var state = eventDataWithState.StateInfo.State;
            var eventName = eventDataWithState.EventPayload.EventName;

            return descriptor.EventTransitionDescriptors.Any(et =>
            {
                return et.FromState == state && et.Event == eventName;
            });
        }

        public EventDescriptor GetEventDescriptor(EventDataWithState eventDataWithState)
        {
            throw new NotImplementedException();
        }
    }
}
