using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using WorkflowModule.Descriptors;
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
            var definitionHelper = GetDefinitionHelper();
            var eventStore = GetEventStore();
            var translator = GetReducerTranslator();

            var stateCalculator = new StateCalculator(eventStore, definitionHelper, translator);

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

            var definitionHelper = GetDefinitionHelper();

            var translator = GetReducerTranslator(new { test = 1 });

            var stateCalculator = new StateCalculator(eventStore, definitionHelper, translator);

            var aggregateId = new Guid();
            var workflowId = "wf1";
            var result = await stateCalculator.GetCurrentStateInfo(aggregateId, workflowId);

            Assert.Equal(1, (result.StateData as dynamic).test);
            Assert.Equal(1, result.CurrentOrderNumber);
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
            var definitionHelper = GetDefinitionHelper();
            var translatorMock = new Mock<IReducerTranslator>();
            translatorMock
                .Setup(x => x.GetReducer(It.Is<EventDescriptor>(ed => ed.Name == "DraftSaved")))
                .Returns(GetReducer(new { test = 1 }));

            translatorMock
                .Setup(x => x.GetReducer(It.Is<EventDescriptor>(ed => ed.Name == "SentToManager")))
                .Returns(GetReducer(new { test = "sample" }));

            var stateCalculator = new StateCalculator(eventStore, definitionHelper, translatorMock.Object);
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
            .Setup(x => x.GetReducer(It.IsAny<EventDescriptor>()))
            .Returns(reducer);

            return translator.Object;
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

        private IWorkflowDefinitionHelper GetDefinitionHelper()
        {
            var definitionHelperMock = new Mock<IWorkflowDefinitionHelper>();
            definitionHelperMock
                .Setup(x => x.GetMatchingTransition(It.IsAny<StateInfo>(), "DraftSaved"))
                .Returns(new ConditionalTransition());

            definitionHelperMock
                .Setup(x => x.GetEventDescriptor(
                    It.Is<EventDataWithState>(ed => ed.EventPayload.EventName == "DraftSaved")
                    ))
                .Returns(new EventDescriptor() { Name = "DraftSaved" });

            definitionHelperMock
                .Setup(x => x.GetEventDescriptor(
                    It.Is<EventDataWithState>(ed => ed.EventPayload.EventName == "SentToManager")
                    ))
                .Returns(new EventDescriptor() { Name = "SentToManager" });

            return definitionHelperMock.Object;
        }
    }
}

