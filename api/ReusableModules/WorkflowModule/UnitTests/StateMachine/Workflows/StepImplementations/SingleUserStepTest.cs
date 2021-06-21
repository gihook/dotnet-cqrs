using System;
using WorkflowModule.StateMachine.Workflows;
using WorkflowModule.StateMachine.Workflows.StepImplementations;
using Xunit;

namespace UnitTests.StateMachine.Workflows.StepImplementations
{
    public class SingleUserStepTest
    {
        [Fact]
        public void StepActions_should_return_actions_based_on_transitions()
        {
            var step = new SingleUserStep();
            step.Id = "s1";
            step.Transitions = new StepTransition[]
            {
                new StepTransition { Id = "transition1", Label = "Send to Manager", NextStepId = "s2" },
                new StepTransition { Id = "transition2", Label = "Send to Manager", NextStepId = "s3" },
                new StepTransition { Id = "transition3", Label = "Send to Manager", NextStepId = "s4" },
            };

            Assert.Collection(step.StepActions, action =>
            {
                Assert.Equal("transition1", action);
            }, action =>
            {
                Assert.Equal("transition2", action);
            }, action =>
            {
                Assert.Equal("transition3", action);
            });
        }

        [Fact]
        public void ExecuteAction_should_return_correct_next_step_id()
        {
            var step = new SingleUserStep();
            step.Id = "s1";
            step.Transitions = new StepTransition[]
            {
                new StepTransition { Id = "transition1", Label = "Send to Manager", NextStepId = "s2" },
                new StepTransition { Id = "transition2", Label = "Send to Manager", NextStepId = "s3" },
                new StepTransition { Id = "transition3", Label = "Send to Manager", NextStepId = "s4" },
            };

            var nextStepId = step.ExecuteAction("transition2", new Guid());

            Assert.Equal("s3", nextStepId);
        }
    }
}
