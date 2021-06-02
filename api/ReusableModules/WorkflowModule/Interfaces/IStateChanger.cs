using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IStateChanger
    {
        string GetNewState(object stateData, EventPayload payload, string workflowId);
    }
}
