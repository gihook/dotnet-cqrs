using System;
using System.Linq;
using System.Threading.Tasks;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;

namespace WorkflowModule.StateMachine
{
    public class StateCalculator : IStateCalculator
    {
        private readonly IEventStore _eventStore;
        private readonly IWorkflowDefinitionHelper _workflowDefinitionHelper;
        private readonly IReducerTranslator _reducerTranslator;

        public StateCalculator(IEventStore eventStore, IWorkflowDefinitionHelper workflowDefinitionHelper, IReducerTranslator reducerTranslator)
        {
            _eventStore = eventStore;
            _workflowDefinitionHelper = workflowDefinitionHelper;
            _reducerTranslator = reducerTranslator;
        }

        public async Task<StateInfo> GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            var storedEvents = await _eventStore.ReadEventStream(aggregateId);

            var newStateInfo = storedEvents.Aggregate(StateInfo.NullState, (stateInfo, payload) =>
            {
                var reducer = GetReducer(payload, stateInfo, workflowId);
                var stateData = reducer.Reduce(stateInfo.StateData, payload);

                return new StateInfo
                {
                    StateData = stateData,
                    CurrentOrderNumber = payload.OrderNumber
                };
            });

            return newStateInfo;
        }

        private IEventReducer GetReducer(EventPayload payload, StateInfo currentStateInfo, string workflowId)
        {
            var eventDataWithState = new EventDataWithState()
            {
                WorkflowId = workflowId,
                StateInfo = currentStateInfo,
                EventPayload = payload
            };
            var eventDescriptor = _workflowDefinitionHelper.GetEventDescriptor(eventDataWithState);
            var reducer = _reducerTranslator.GetReducer(eventDescriptor);

            return reducer;
        }

        public Task<StateInfo> ApplyEvent(EventPayload payload, string workflowId)
        {
            return Task.FromResult<StateInfo>(null);
        }
    }
}
