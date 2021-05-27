using Moq;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using Xunit;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class EventValidationExecutorTest
    {
        [Fact]
        public void Should_Return_Error_When_Order_Number_Is_Wrong()
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
        public void Should_Return_State_Error_When_Event_Is_Not_Allowed()
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

        private WorkflowDefinitionHelper GetWorkflowDefinitionHelper(bool returnValue)
        {
            var workflowDefinitionHelper = new Mock<WorkflowDefinitionHelper>();
            workflowDefinitionHelper
                .Setup(wdh => wdh.EventIsAllowed(It.IsAny<string>(), It.IsAny<StateInfo>(), It.IsAny<string>()))
                .Returns(returnValue);

            return workflowDefinitionHelper.Object;
        }
    }
}
