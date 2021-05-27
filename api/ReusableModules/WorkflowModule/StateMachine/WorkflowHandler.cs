using System;
using System.Linq;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowHandler
    {
        private readonly StateCalculator _stateCalculator;
        private readonly IEventValidationExecutor _eventValidationExecutor;

        public WorkflowHandler(StateCalculator stateCalculator, IEventValidationExecutor eventValidationExecutor)
        {
            _stateCalculator = stateCalculator;
            _eventValidationExecutor = eventValidationExecutor;
        }

        public StateInfo GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            return _stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);
        }

        public StateInfo ExecuteEvent(EventPayload payload, string workflowId)
        {
            var currentState = GetCurrentStateInfo(payload.AggregateId, workflowId);
            var validationErrors = _eventValidationExecutor.ValidateEvent(currentState, payload, workflowId);

            if (validationErrors.Count() != 0) throw new EventValidationException(validationErrors);

            return _stateCalculator.ApplyEvent(payload, workflowId);
        }
    }
}
