using System.Collections.Generic;
using Moq;
using WorkflowModule.Descriptors;
using WorkflowModule.Exceptions;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class WorkflowDefinitionHelperTest
    {
        [Fact]
        public void EventIsAllowed_should_return_true_when_fsm_allows_it()
        {
            var stateInfo = new StateInfo()
            {
                State = "Draft"
            };
            var payload = new EventPayload()
            {
                EventName = "SentToManager"
            };
            var data = new EventDataWithState()
            {
                WorkflowId = "wf1",
                StateInfo = stateInfo,
                EventPayload = payload
            };

            var definitionLoader = GetDefinitionLoader();

            var helper = new WorkflowDefinitionHelper(definitionLoader);
            var result = helper.EventIsAllowed(data);

            Assert.True(result);
        }

        [Fact]
        public void EventIsAllowed_should_return_false_when_there_is_no_matching_transition()
        {
            var stateInfo = new StateInfo()
            {
                State = "UnknownState"
            };
            var payload = new EventPayload()
            {
                EventName = "SentToManager"
            };
            var data = new EventDataWithState()
            {
                WorkflowId = "wf1",
                StateInfo = stateInfo,
                EventPayload = payload
            };

            var definitionLoader = GetDefinitionLoader();

            var helper = new WorkflowDefinitionHelper(definitionLoader);
            var result = helper.EventIsAllowed(data);

            Assert.False(result);
        }

        [Fact]
        public void EventIsAllowed_should_throw_UnknownWorkflowExeception_when_there_is_no_definition()
        {
            var data = new EventDataWithState()
            {
                WorkflowId = "UnknownWorkflowId"
            };

            var definitionLoader = GetDefinitionLoader();

            var helper = new WorkflowDefinitionHelper(definitionLoader);

            Assert.Throws<UnknownWorkflowException>(() => helper.EventIsAllowed(data));
        }

        [Fact]
        public void GetEventDescriptor_should_return_correct_event_descriptor()
        {
            var payload = new EventPayload()
            {
                EventName = "SentToManager"
            };
            var data = new EventDataWithState()
            {
                WorkflowId = "wf1",
                EventPayload = payload
            };

            var definitionLoader = GetDefinitionLoader();

            var helper = new WorkflowDefinitionHelper(definitionLoader);
            var result = helper.GetEventDescriptor(data);

            Assert.Equal("ExampleReducer", result.ReducerDescriptor.Type);
        }

        [Fact]
        public void GetEventDescriptor_should_throw_UnknownEventExeception_for_non_exsisting_event()
        {
            var payload = new EventPayload()
            {
                EventName = "NonExistingEvent"
            };
            var data = new EventDataWithState()
            {
                WorkflowId = "wf1",
                EventPayload = payload
            };

            var definitionLoader = GetDefinitionLoader();

            var helper = new WorkflowDefinitionHelper(definitionLoader);

            Assert.Throws<UnknownEventException>(() => helper.GetEventDescriptor(data));
        }

        [Fact]
        public void GetMatchingEventTransitionDescriptor_should_return_matched_transition()
        {
            var definitionLoader = GetDefinitionLoader();
            var helper = new WorkflowDefinitionHelper(definitionLoader);

            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload
                {
                    EventName = "SentToManager"
                },
                WorkflowId = "wf1",
                StateInfo = new StateInfo
                {
                    State = "Draft"
                }
            };

            var result = helper.GetMatchingEventTransitionDescriptor(eventDataWithState);

            Assert.NotNull(result);
            Assert.Equal("Draft", result.FromState);
            Assert.Equal("SentToManager", result.Event);
        }

        [Fact]
        public void GetMatchingEventTransitionDescriptor_should_throw_NonExistingTransitionException()
        {
            var definitionLoader = GetDefinitionLoader();
            var helper = new WorkflowDefinitionHelper(definitionLoader);

            var eventDataWithState = new EventDataWithState
            {
                EventPayload = new EventPayload
                {
                    EventName = "NonExistinEventHappened"
                },
                WorkflowId = "wf1",
                StateInfo = new StateInfo
                {
                    State = "Draft"
                }
            };

            Assert.Throws<NonExistingTransitionException>(() =>
            {
                var result = helper.GetMatchingEventTransitionDescriptor(eventDataWithState);
            });
        }

        private IWorkflowDefinitionLoader GetDefinitionLoader()
        {
            var definitionLoaderMock = new Mock<IWorkflowDefinitionLoader>();
            var eventDescriptors = new EventDescriptor[]
            {
                new EventDescriptor()
                {
                    Name = "SentToManager",
                    ReducerDescriptor = new ReducerDescriptor()
                    {
                        Type = "ExampleReducer"
                    }
                }
            };
            var eventTransitions = new EventTransitionDescriptor[]
            {
                new EventTransitionDescriptor()
                {
                    FromState ="Draft",
                    Event = "SentToManager"
                }
            };

            var definitions = new Dictionary<string, WorkflowDescriptor>()
            {
                ["wf1"] = new WorkflowDescriptor()
                {
                    EventTransitionDescriptors = eventTransitions,
                    EventDescriptors = eventDescriptors
                }
            };

            definitionLoaderMock.Setup(x => x.LoadWorkflows()).Returns(definitions);

            return definitionLoaderMock.Object;
        }
    }
}
