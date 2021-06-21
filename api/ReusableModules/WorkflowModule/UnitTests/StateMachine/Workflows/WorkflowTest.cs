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

            Assert.Equal("accepted-step", wf.CurrentStep.Id);
            Assert.True(wf.IsCompleted);
        }

        [Fact]
        public void AreValidSteps_should_return_false_without_originator_step()
        {
            var steps = new Step[0];
            Assert.False(Workflow.AreValidSteps(steps));
        }

        [Fact]
        public void AreValidSteps_should_return_false_without_accepted_and_rejected_steps()
        {
            var originatorStepMock = new Mock<Step>();
            var originatorStep = originatorStepMock.Object;
            originatorStep.Id = "originator-step";

            var steps = new Step[] { originatorStep };
            Assert.False(Workflow.AreValidSteps(steps));
        }
    }
}
