using System.Collections.Generic;
using Moq;
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
                Data = new Dictionary<string, object>()
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
                Data = new Dictionary<string, object>()
                {
                    ["first"] = "this string is not an integer"
                }
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
                Data = new Dictionary<string, object>()
            };

            var mock = new Mock<IValidatorTranslator>();
            mock.Setup(x => x.CanParse(It.IsAny<string>(), It.IsAny<object>())).Returns(true);
            mock.Setup(x => x.GetValidator(It.IsAny<InputValidatorDescriptor>())).Returns(x => false);

            var validationExecutor = GetInstance(true, null, mock.Object);
            var validationErrors = validationExecutor.ValidateEvent(new StateInfo(), payload, "");

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("ValidatiorFunctionError", error.Id);
            });
        }

        private EventValidationExecutor GetInstance(
            bool isEventAllowed,
            Dictionary<string, string> inputs,
            IValidatorTranslator validatorTranslatorInstance = null)
        {
            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(isEventAllowed, inputs);
            var validatorTranslator = validatorTranslatorInstance ?? GetValidatorTranslator();
            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper, validatorTranslator);

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
                .Setup(wdh => wdh.EventIsAllowed(It.IsAny<string>(), It.IsAny<StateInfo>(), It.IsAny<string>()))
                .Returns(returnValue);


            var eventDescriptor = new EventDescriptor();
            eventDescriptor.ValidatorDescriptors = new InputValidatorDescriptor[] { new InputValidatorDescriptor() };
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
