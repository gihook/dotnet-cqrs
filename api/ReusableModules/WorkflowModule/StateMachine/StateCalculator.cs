using System;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class StateCalculator : IStateCalculator
    {
        public StateInfo GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            return null;
        }

        public StateInfo ApplyEvent(EventPayload payload, string workflowId)
        {
            return null;
        }
    }
}
