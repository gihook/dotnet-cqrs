using System;
using System.Collections.Generic;
using Moq;
using Newtonsoft.Json.Linq;
using WorkflowModule.Descriptors;
using WorkflowModule.Interfaces;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class EventValidationExecutorTest
    {
        [Fact]
        public void Return_Error_When_Order_Number_Is_Wrong()
        {
            var validationExecutor = GetInstance(true, null);

            var payload = new EventPayload()
            {
                OrderNumber = 0
            };

            var workflowId = "SampleWorkflow";

            var stateInfo = new StateInfo()
            {
                CurrentOrderNumber = 1
            };

            var validationErrors = validationExecutor.ValidateEvent(stateInfo, payload, workflowId);

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("OrderNumberConfilict", error.Id);
            });
        }

        [Fact]
        public void Return_State_Error_When_Event_Is_Not_Allowed()
        {
            var payload = new EventPayload()
            {
                OrderNumber = 2,
                EventName = "MovedToSecondState"
            };

            var workflowId = "SampleWorkflow";

            var stateInfo = new StateInfo()
            {
                CurrentOrderNumber = 1,
                State = "SecondState"
            };

            var validationExecutor = GetInstance(false, null);

            var validationErrors = validationExecutor.ValidateEvent(stateInfo, payload, workflowId);

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("EventNotAllowed", error.Id);
            });
        }

        [Fact]
        public void Return_Event_Parameter_Error()
        {
            var payload = new EventPayload()
            {
                OrderNumber = 2,
                EventName = "MovedToSecondState",
                Data = new JObject()
            };

            var workflowId = "SampleWorkflow";

            var stateInfo = new StateInfo()
            {
                CurrentOrderNumber = 1,
                State = "SecondState"
            };

            var inputs = new Dictionary<string, string>()
            {
                ["first"] = "string",
                ["second?"] = "int"
            };

            var validationExecutor = GetInstance(false, inputs);

            var validationErrors = validationExecutor.ValidateEvent(stateInfo, payload, workflowId);

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("EventNotAllowed", error.Id);
            }, error =>
            {
                Assert.Equal("RequiredInputParameterNotProvided", error.Id);
            });
        }

        [Fact]
        public void Return_Type_Validation_Errors()
        {
            var payload = new EventPayload()
            {
                OrderNumber = 1,
                EventName = "ItemCreated",
                Data = new JObject() { ["first"] = "this is not an integer" }
            };

            var inputs = new Dictionary<string, string>()
            {
                ["first"] = "int",
            };

            var validationExecutor = GetInstance(true, inputs);

            var validationErrors = validationExecutor.ValidateEvent(new StateInfo(), payload, "");

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("InvalidInputType", error.Id);
                Assert.Equal("first", error.ParameterName);
            });
        }

        [Fact]
        public void Return_Validator_Function_Error()
        {
            var payload = new EventPayload()
            {
                OrderNumber = 1,
                Data = new JObject()
            };

            var mock = new Mock<IValidatorTranslator>();
            mock.Setup(x => x.CanParse(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
            mock.Setup(x => x.GetValidator(It.IsAny<InputValidatorDescriptor>())).Returns(x => false);

            var validationExecutor = GetInstance(true, null, mock.Object);
            var validationErrors = validationExecutor.ValidateEvent(new StateInfo(), payload, "");

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("ValidatiorFunctionError", error.Id);
                Assert.Equal("IsGreaterThen", error.ParameterName);
            });
        }

        [Fact]
        public void Validator_Function_Is_Called_With_Parsed_Parameters()
        {
            var payload = new EventPayload()
            {
                OrderNumber = 1,
                Data = new JObject()
            };

            var mockValidatorFunction = new Mock<Func<object[], bool>>();
            mockValidatorFunction.Setup(f => f(It.IsAny<object[]>())).Returns(true);

            var mock = new Mock<IValidatorTranslator>();
            mock.Setup(x => x.CanParse(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
            mock.Setup(x => x.GetValidator(It.IsAny<InputValidatorDescriptor>())).Returns(mockValidatorFunction.Object);

            var validationExecutor = GetInstance(true, null, mock.Object);
            var validationErrors = validationExecutor.ValidateEvent(new StateInfo(), payload, "");

            mockValidatorFunction.Verify(f => f(new object[] { 4, 5 }), Times.Once);
        }

        private EventValidationExecutor GetInstance(
            bool isEventAllowed,
            Dictionary<string, string> inputs,
            IValidatorTranslator validatorTranslatorInstance = null)
        {
            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(isEventAllowed, inputs);
            var validatorTranslator = validatorTranslatorInstance ?? GetValidatorTranslator();
            var parameterTranslatorMock = new Mock<IParameterTranslator>();
            parameterTranslatorMock.Setup(x => x.GetParameterValue("EVENT_INPUTS.test", It.IsAny<EventDataWithState>())).Returns(4);
            parameterTranslatorMock.Setup(x => x.GetParameterValue("5", It.IsAny<EventDataWithState>())).Returns(5);

            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper, validatorTranslator, parameterTranslatorMock.Object);

            return validationExecutor;
        }

        private IValidatorTranslator GetValidatorTranslator()
        {
            var mock = new Mock<IValidatorTranslator>();
            mock.Setup(x => x.CanParse("int", It.IsAny<object>())).Returns(false);
            mock.Setup(x => x.GetValidator(It.IsAny<InputValidatorDescriptor>())).Returns(x => true);

            return mock.Object;
        }

        private WorkflowDefinitionHelper GetWorkflowDefinitionHelper(bool returnValue, Dictionary<string, string> inputs = null)
        {
            var workflowDefinitionHelper = new Mock<WorkflowDefinitionHelper>();
            workflowDefinitionHelper
                .Setup(wdh => wdh.EventIsAllowed(It.IsAny<EventDataWithState>()))
                .Returns(returnValue);


            var eventDescriptor = new EventDescriptor();
            eventDescriptor.ValidatorDescriptors = new InputValidatorDescriptor[]
            {
                new InputValidatorDescriptor()
                {
                    Type = "IsGreaterThen",
                    Params = new string[] { "EVENT_INPUTS.test", "5"}
                }
            };
            eventDescriptor.Inputs = inputs == null
                                         ? new Dictionary<string, string>()
                                         : inputs;
            workflowDefinitionHelper
                .Setup(wdh => wdh.GetEventDescriptor(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(eventDescriptor);

            return workflowDefinitionHelper.Object;
        }
    }
}
