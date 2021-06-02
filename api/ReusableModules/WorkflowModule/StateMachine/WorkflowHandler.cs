using System;
using System.Linq;
using System.Threading.Tasks;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class WorkflowHandler
    {
        private readonly IStateCalculator _stateCalculator;
        private readonly IEventValidationExecutor _eventValidationExecutor;

        public WorkflowHandler(IStateCalculator stateCalculator, IEventValidationExecutor eventValidationExecutor)
        {
            _stateCalculator = stateCalculator;
            _eventValidationExecutor = eventValidationExecutor;
        }

        public Task<StateInfo> GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            return _stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);
        }

        public async Task<StateInfo> ExecuteEvent(EventPayload payload, string workflowId)
        {
            var currentState = await GetCurrentStateInfo(payload.AggregateId, workflowId);
            var validationErrors = _eventValidationExecutor.ValidateEvent(currentState, payload, workflowId);

            if (validationErrors.Count() != 0) throw new EventValidationException(validationErrors);

            return await _stateCalculator.ApplyEvent(payload, workflowId);
        }
    }
}
