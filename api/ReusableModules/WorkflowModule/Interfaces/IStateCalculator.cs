using System;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public interface IStateCalculator
    {
        StateInfo GetCurrentStateInfo(Guid aggregateId, string workflowId);
        StateInfo ApplyEvent(EventPayload payload, string workflowId);
    }
}
