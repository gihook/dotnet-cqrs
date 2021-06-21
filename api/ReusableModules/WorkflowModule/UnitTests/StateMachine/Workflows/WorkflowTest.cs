using System;
using System.Collections.Generic;
using Moq;
using WorkflowModule.StateMachine.Workflows;
using WorkflowModule.StateMachine.Workflows.StepImplementations;
using Xunit;

namespace UnitTests.StateMachine.Workflows
{
    public class WorkflowTest
    {
        [Fact]
        public void CurrentStep_should_be_originator_upon_creation()
        {
            var steps = new List<Step>()
            {
                new SingleUserStep() { Id = "originator-step" },
                FinalStep.Accepted(),
                FinalStep.Rejected()
            };

            var wf = new Workflow(steps);

            Assert.Equal("originator-step", wf.CurrentStep.Id);
            Assert.True(wf.Steps.ContainsKey("originator-step"));
            Assert.True(wf.Steps.ContainsKey("accepted-step"));
            Assert.True(wf.Steps.ContainsKey("rejected-step"));

            var finalApproved = wf.Steps["accepted-step"];
            Assert.Equal(StepStatus.Accepted, finalApproved.StepStatus);

            var finalRejected = wf.Steps["rejected-step"];
            Assert.Equal(StepStatus.Rejected, finalRejected.StepStatus);
            Assert.False(wf.IsCompleted);
        }

        [Fact]
        public void ExecuteAction_should_move_workflow_to_next_step()
        {
            var actionName = "transition:2789744";
            var userId = Guid.NewGuid();
            var originatorStepMock = new Mock<Step>();
            originatorStepMock
                .Setup(x => x.ExecuteAction(actionName, userId))
                .Returns("accepted-step");

            var originatorStep = originatorStepMock.Object;
            originatorStep.Id = "originator-step";

            var acceptedStep = FinalStep.Accepted();

            var wf = new Workflow(new Step[] { originatorStep, acceptedStep });
            wf.ExecuteAction(actionName, userId);

            Assert.Equal(wf.CurrentStep.Id, "accepted-step");
            Assert.True(wf.IsCompleted);
        }
    }
}
