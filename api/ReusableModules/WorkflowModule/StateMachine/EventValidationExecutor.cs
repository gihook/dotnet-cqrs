using System.Collections.Generic;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class EventValidationExecutor : IEventValidationExecutor
    {
        private readonly WorkflowDefinitionHelper _workflowDefinitionHelper;

        public EventValidationExecutor(WorkflowDefinitionHelper workflowDefinitionHelper)
        {
            _workflowDefinitionHelper = workflowDefinitionHelper;
        }

        public IEnumerable<ValidationError> ValidateEvent(StateInfo currentState, EventPayload payload, string workflowId)
        {
            var errors = new List<ValidationError>();

            var isInConflict = currentState.CurrentOrderNumber + 1 != payload.OrderNumber;
            if (isInConflict) errors.Add(new ValidationError() { Id = "OrderNumberConfilict" });

            var eventIsAllowed = _workflowDefinitionHelper
                                     .EventIsAllowed(payload.EventName, currentState, workflowId);

            if (!eventIsAllowed)
            {
                errors.Add(new ValidationError() { Id = "EventNotAllowed" });
            }

            return errors;
        }
    }
}
