using System;
using System.Threading.Tasks;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public interface IStateCalculator
    {
        Task<StateInfo> GetCurrentStateInfo(Guid aggregateId, string workflowId);
        Task<EventPayload> ApplyEvent(EventPayload payload);
    }
}
