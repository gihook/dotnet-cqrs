using System;
using Moq;
using WorkflowModule.Descriptors;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class StateChangerTest
    {
        [Fact]
        public void GetNewState_should_return_only_possible_matched_state()
        {
            var definitionHelper = GetWorkflowDefinitionHelper();
            var validatorTranslator = GetValidatorTranslator();
            var parameterTranslator = GetParameterTranslator();

            var stateChanger = new StateChanger(definitionHelper, validatorTranslator, parameterTranslator);

            var input = new EventDataWithState();

            var result = stateChanger.GetNewState(input);

            Assert.Equal("WaitingForManager", result);
        }

        [Fact]
        public void GetNewState_should_return_old_state_if_not_matched()
        {
            var definitionHelperMock = new Mock<IWorkflowDefinitionHelper>();
            definitionHelperMock
            .Setup(x => x.GetMatchingEventTransitionDescriptor(It.IsAny<EventDataWithState>()))
            .Returns(new EventTransitionDescriptor
            {
                ConditionalTransitions = new ConditionalTransition[0]
            });

            var validatorTranslator = GetValidatorTranslator();
            var parameterTranslator = GetParameterTranslator();

            var stateChanger = new StateChanger(definitionHelperMock.Object, validatorTranslator, parameterTranslator);

            var input = new EventDataWithState()
            {
                StateInfo = new StateInfo
                {
                    State = "CurrentState"
                }
            };

            var result = stateChanger.GetNewState(input);

            Assert.Equal("CurrentState", result);
        }

        [Fact]
        public void GetNewState_should_return_first_matched_state()
        {
            var parameters = new string[] { "STATE_DATA.count", "4" };
            var definitionHelper = GetWorkflowDefinitionHelper("IsGreaterThan", parameters);

            var input = new EventDataWithState()
            {
                StateInfo = new StateInfo
                {
                    State = "CurrentState",
                    StateData = new { count = 5 }
                }
            };

            var validatorTranslator = GetValidatorTranslator();
            var parameterTranslator = GetParameterTranslator();

            var stateChanger = new StateChanger(definitionHelper, validatorTranslator, parameterTranslator);

            var result = stateChanger.GetNewState(input);

            Assert.Equal("WaitingForManager", result);
        }

        private IParameterTranslator GetParameterTranslator()
        {
            var parameterTranslatorMock = new Mock<IParameterTranslator>();
            parameterTranslatorMock
                .Setup(p => p.GetParameterValue("STATE_DATA.count", It.IsAny<EventDataWithState>()))
                .Returns(5);

            parameterTranslatorMock
                .Setup(p => p.GetParameterValue("4", It.IsAny<EventDataWithState>()))
                .Returns(4);

            return parameterTranslatorMock.Object;
        }

        private IValidatorTranslator GetValidatorTranslator()
        {
            var validatorTranslatorMock = new Mock<IValidatorTranslator>();
            validatorTranslatorMock
                .Setup(v => v.GetValidationFunction("IsGreaterThan"))
                .Returns(parameters =>
                {
                    var param1 = Convert.ToInt32(parameters[0]);
                    var param2 = Convert.ToInt32(parameters[1]);

                    return param1 > param2;
                });

            return validatorTranslatorMock.Object;
        }

        private IWorkflowDefinitionHelper GetWorkflowDefinitionHelper(string condition = null, string[] parameters = null)
        {
            var matchedTransition = new ConditionalTransition
            {
                ToState = "WaitingForManager",
                Condition = condition ?? "TRUE",
                Params = parameters
            };

            var conditionalTransitions = new ConditionalTransition[]
            {
                matchedTransition
            };

            var eventTransition = new EventTransitionDescriptor()
            {
                FromState = "Draft",
                Event = "SentToManager",
                ConditionalTransitions = conditionalTransitions
            };

            var definitionHelperMock = new Mock<IWorkflowDefinitionHelper>();
            definitionHelperMock
                .Setup(x => x.GetMatchingEventTransitionDescriptor(It.IsAny<EventDataWithState>()))
                .Returns(eventTransition);

            return definitionHelperMock.Object;
        }
    }
}
