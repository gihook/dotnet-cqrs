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
        private readonly IReducerTranslator _reducerTranslator;
        private readonly IStateChanger _stateChanger;

        public StateCalculator(IEventStore eventStore, IReducerTranslator reducerTranslator, IStateChanger stateChanger)
        {
            _eventStore = eventStore;
            _reducerTranslator = reducerTranslator;
            _stateChanger = stateChanger;
        }

        public async Task<StateInfo> GetCurrentStateInfo(Guid aggregateId, string workflowId)
        {
            var storedEvents = await _eventStore.ReadEventStream(aggregateId);

            var newStateInfo = storedEvents.Aggregate(StateInfo.NullState, (stateInfo, payload) =>
            {
                var reducer = _reducerTranslator.GetReducer(payload, workflowId);
                var stateData = reducer.Reduce(stateInfo.StateData, payload);

                stateInfo.StateData = stateData;
                stateInfo.CurrentOrderNumber = payload.OrderNumber;

                var eventDataWithState = new EventDataWithState
                {
                    StateInfo = stateInfo,
                    EventPayload = payload,
                    WorkflowId = workflowId
                };
                var newState = _stateChanger.GetNewState(eventDataWithState);

                return new StateInfo
                {
                    StateData = stateData,
                    CurrentOrderNumber = payload.OrderNumber,
                    State = newState
                };
            });

            return newStateInfo;
        }

        public Task<StateInfo> ApplyEvent(EventPayload payload, string workflowId)
        {
            return Task.FromResult<StateInfo>(null);
        }
    }
}
