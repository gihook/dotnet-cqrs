using System.Collections.Generic;
using WorkflowModule.Models;

namespace WorkflowModule.Interfaces
{
    public interface IEventValidationExecutor
    {
        IEnumerable<ValidationError> ValidateEvent(StateInfo currentState, EventPayload payload, string workflowId);
    }
}
