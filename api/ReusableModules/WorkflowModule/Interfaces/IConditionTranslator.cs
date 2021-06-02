using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IConditionTranslator
    {
        string GetNewState(object stateData, EventPayload payload, string workflowId);
    }
}
