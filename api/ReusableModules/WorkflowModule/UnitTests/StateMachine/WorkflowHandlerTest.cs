using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using WorkflowModule.Exceptions;
using WorkflowModule.Models;
using WorkflowModule.StateMachine;
using WorkflowModule.Interfaces;
using Xunit;
using System.Threading.Tasks;

namespace UnitTests.WorkflowModule.StateMachine
{
    public class WorkflowHandlerTest
    {
        [Fact]
        public async Task Should_Return_Correct_State_Info()
        {
            var aggregateId = Guid.NewGuid();
            var workflowId = "SampleWorkflow";

            var stateCalculator = new Mock<IStateCalculator>();
            stateCalculator
                .Setup(sc => sc.GetCurrentStateInfo(aggregateId, workflowId))
                .ReturnsAsync(StateInfo.NullState);

            var workflowHandler = new WorkflowHandler(stateCalculator.Object, null);

            var stateInfo = await workflowHandler.GetCurrentStateInfo(aggregateId, workflowId);

            Assert.Equal(StateInfo.NullState.State, stateInfo.State);
        }

        [Fact]
        public async Task Should_Correctly_Execute_Event()
        {
            var aggregateId = Guid.NewGuid();
            var workflowId = "SampleWorkflow";

            var stateCalculator = new Mock<IStateCalculator>();
            stateCalculator
                .Setup(sc => sc.ApplyEvent(It.IsAny<EventPayload>(), workflowId))
                .ReturnsAsync(new StateInfo() { State = "NewState" });

            var eventValidationExecutor = new Mock<IEventValidationExecutor>();
            eventValidationExecutor
                    .Setup(ev => ev.ValidateEvent(It.IsAny<StateInfo>(), It.IsAny<EventPayload>(), workflowId))
                    .Returns(Enumerable.Empty<ValidationError>());

            var workflowHandler = new WorkflowHandler(stateCalculator.Object, eventValidationExecutor.Object);

            var payload = new EventPayload();
            payload.AggregateId = aggregateId;

            var stateInfo = await workflowHandler.ExecuteEvent(payload, workflowId);

            Assert.Equal("NewState", stateInfo.State);
        }

        [Fact]
        public void Should_Raise_ValidationException_When_Validation_Fails()
        {
            var aggregateId = Guid.NewGuid();
            var workflowId = "SampleWorkflow";

            var stateCalculator = new Mock<IStateCalculator>();

            var eventValidationExecutor = new Mock<IEventValidationExecutor>();
            eventValidationExecutor
            .Setup(ev => ev.ValidateEvent(It.IsAny<StateInfo>(), It.IsAny<EventPayload>(), workflowId))
                    .Returns(new List<ValidationError>() { new ValidationError() });

            var workflowHandler = new WorkflowHandler(stateCalculator.Object, eventValidationExecutor.Object);

            var payload = new EventPayload();
            payload.AggregateId = aggregateId;

            Assert.ThrowsAsync<EventValidationException>(() => workflowHandler.ExecuteEvent(payload, workflowId));
        }
    }
}
