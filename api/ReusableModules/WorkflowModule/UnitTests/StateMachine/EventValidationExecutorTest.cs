using System.Collections.Generic;
using Moq;
using WorkflowModule.Descriptors;
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
            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(true);
            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper);

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

            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(false);

            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper);

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
            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(false, inputs);

            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper);

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
            var workflowDefinitionHelper = GetWorkflowDefinitionHelper(true, inputs);

            var validationExecutor = new EventValidationExecutor(workflowDefinitionHelper);

            var validationErrors = validationExecutor.ValidateEvent(new StateInfo(), payload, "");

            Assert.Collection(validationErrors, error =>
            {
                Assert.Equal("InvalidInputType", error.Id);
                Assert.Equal("first", error.ParameterName);
            });
        }

        private WorkflowDefinitionHelper GetWorkflowDefinitionHelper(bool returnValue, Dictionary<string, string> inputs = null)
        {
            var workflowDefinitionHelper = new Mock<WorkflowDefinitionHelper>();
            workflowDefinitionHelper
                .Setup(wdh => wdh.EventIsAllowed(It.IsAny<string>(), It.IsAny<StateInfo>(), It.IsAny<string>()))
                .Returns(returnValue);


            var eventDescriptor = new EventDescriptor();
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
