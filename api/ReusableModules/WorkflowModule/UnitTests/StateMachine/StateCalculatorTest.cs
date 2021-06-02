using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class StateCalculatorTest
    {
        [Fact]
        public async Task GetCurrentStateInfo_should_return_null_state_initially()
        {
            var eventStore = GetEventStore();
            var translator = GetReducerTranslator();

            var conditionTranslator = GetConditionTranslator();
            var stateCalculator = new StateCalculator(eventStore, translator, conditionTranslator);

            var aggregateId = new Guid();
            var workflowId = "wf1";

            var result = await stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);

            Assert.Equal("NULL_STATE", result.State);
            Assert.Null(result.StateData);
            Assert.Equal(0, result.CurrentOrderNumber);
        }

        [Fact]
        public async Task GetCurrentStateInfo_should_return_current_state_after_one_event()
        {
            var firstEvent = new EventPayload()
            {
                EventName = "DraftSaved",
                OrderNumber = 1
            };
            var events = new EventPayload[] { firstEvent };
            var eventStore = GetEventStore(events);

            var translator = GetReducerTranslator(new { test = 1 });

            var conditionTranslator = GetConditionTranslator();
            var stateCalculator = new StateCalculator(eventStore, translator, conditionTranslator);

            var aggregateId = new Guid();
            var workflowId = "wf1";
            var result = await stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);

            Assert.Equal(1, (result.StateData as dynamic).test);
            Assert.Equal(1, result.CurrentOrderNumber);
            Assert.Equal("NewState", result.State);
        }

        [Fact]
        public async Task GetCurrentStateInfo_should_return_current_state_after_two_events()
        {
            var firstEvent = new EventPayload()
            {
                EventName = "DraftSaved",
                OrderNumber = 1
            };
            var secondEvent = new EventPayload()
            {
                EventName = "SentToManager",
                OrderNumber = 2
            };
            var events = new EventPayload[] { firstEvent, secondEvent };
            var eventStore = GetEventStore(events);
            var translatorMock = new Mock<IReducerTranslator>();
            translatorMock
                .Setup(x => x.GetReducer(It.Is<EventPayload>(ed => ed.EventName == "DraftSaved"), "wf1"))
                .Returns(GetReducer(new { test = 1 }));

            translatorMock
                .Setup(x => x.GetReducer(It.Is<EventPayload>(ed => ed.EventName == "SentToManager"), "wf1"))
                .Returns(GetReducer(new { test = "sample" }));

            var conditionTranslator = GetConditionTranslator();
            var stateCalculator = new StateCalculator(eventStore, translatorMock.Object, conditionTranslator);

            var aggregateId = new Guid();
            var workflowId = "wf1";
            var result = await stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);

            Assert.Equal("sample", (result.StateData as dynamic).test);
            Assert.Equal(2, result.CurrentOrderNumber);
        }

        private IReducerTranslator GetReducerTranslator(object reducerResult = null)
        {
            var reducer = GetReducer(reducerResult);

            var translator = new Mock<IReducerTranslator>();
            translator
            .Setup(x => x.GetReducer(It.IsAny<EventPayload>(), "wf1"))
            .Returns(reducer);

            return translator.Object;
        }

        private IConditionTranslator GetConditionTranslator()
        {
            var matcherMock = new Mock<IConditionMatcher>();

            var mock = new Mock<IConditionTranslator>();
            mock
            .Setup(x => x.GetNewState(It.IsAny<object>(), It.IsAny<EventPayload>(), "wf1"))
            .Returns("NewState");

            return mock.Object;
        }

        private IEventReducer GetReducer(object reducerResult)
        {
            var reducer = new Mock<IEventReducer>();
            reducer
            .Setup(x => x.Reduce(It.IsAny<object>(), It.IsAny<EventPayload>()))
            .Returns(reducerResult ?? new object());

            return reducer.Object;
        }

        private IEventStore GetEventStore(IEnumerable<EventPayload> storedEvents = null)
        {
            var eventStoreMock = new Mock<IEventStore>();
            eventStoreMock
                .Setup(x => x.ReadEventStream(It.IsAny<Guid>()))
                .ReturnsAsync(storedEvents ?? new EventPayload[0]);

            return eventStoreMock.Object;
        }
    }
}

