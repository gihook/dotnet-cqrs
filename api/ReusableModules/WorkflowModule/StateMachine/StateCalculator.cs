using System;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class StateCalculator
    {
        public virtual StateInfo GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            return null;
        }

        public virtual StateInfo ApplyEvent(EventPayload payload, string workflowId)
        {
            return null;
        }
    }
}
